using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Exceptions;
using learning_center_back.Tutorials.Interfaces.REST.Transform;

namespace learning_center_back.Tutorials.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController(IBookQueryService bookQueryService, IBookCommandService bookCommandService) : ControllerBase
    {
        private readonly IBookQueryService _bookQueryService = bookQueryService;
        private readonly IBookCommandService _bookCommandService = bookCommandService;

        // GET: api/Book
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _bookQueryService.Handle(new GetAllBooksQuery());
            return result.Any() ? Ok(result.Select(BookResourceFromEntityAssembler.ToResourceFromEntity)) : NotFound("No books found.");
        }

        // GET: api/Book/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            var result = await _bookQueryService.Handle(new GetBookByIdQuery(id));
            return result != null ? Ok(BookResourceFromEntityAssembler.ToResourceFromEntity(result)) : NotFound($"Book with ID {id} not found.");
        }

        // POST: api/Book
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookCommand command)
        {
            if (string.IsNullOrWhiteSpace(command?.Name)) return BadRequest("Book name cannot be empty.");

            try
            {
                await _bookCommandService.Handle(command);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ValidationException ex) { return UnprocessableEntity(ex.Message); }
            catch (ArgumentException ex) { return UnprocessableEntity(ex.Message); }
            catch (NotChapterFoundException ex) { return BadRequest(ex.Message); }
            catch (DuplicateNameException) { return Conflict("A book with the same name already exists."); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError); }
        }

        // PUT: api/Book/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateBookCommand command)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            try
            {
                await _bookCommandService.Handle(command, id);
                return Ok();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError); }
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            try
            {
                await _bookCommandService.Handle(new DeleteBookCommand(id));
                return NoContent();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError); }
        }
    }
}
