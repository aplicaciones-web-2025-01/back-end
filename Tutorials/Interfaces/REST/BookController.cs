using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Exceptions;
using learning_center_back.Tutorials.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_back.Tutorials.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController(IBookQueryService bookQueryService, IBookCommandService bookCommandService)
        : ControllerBase
    {
        private readonly IBookQueryService _bookQueryService = bookQueryService ?? throw new ArgumentNullException(nameof(bookQueryService));
        private readonly IBookCommandService _bookCommandService = bookCommandService ?? throw new ArgumentNullException(nameof(bookCommandService));

        // GET: api/Book
        public async Task<IActionResult> GetAsync()
        {
            var query = new GetAllBooksQuery();
            var result = await _bookQueryService.Handle(query);

            if (!result.Any()) return NotFound("No books found.");

            var resources = result.Select(BookResourceFromEntityAssembler.ToResourceFromEntity).ToList();
            return Ok(resources);
        }

        // GET: api/Book/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            try
            {
                var query = new GetBookByIdQuery(id);
                var result = await _bookQueryService.Handle(query);

                return result != null ? Ok(BookResourceFromEntityAssembler.ToResourceFromEntity(result)) : NotFound($"Book with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Book
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBookCommand command)
        {
            if (command == null) return BadRequest("Invalid book data.");


            //Error
            //if ( command.Name != string.Empty)
            //     return BadRequest("Book name is invalid.");


            try
            {
                await _bookCommandService.Handle(command);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (NotChapterFoundException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (DuplicateNameException)
            {
                return Conflict("A book with the same name already exists.");
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Book/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateBookCommand UpdateBookCommand)
        {
            try
            {
                _bookCommandService.Handle(UpdateBookCommand, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            try
            {
                var command = new DeleteBookCommand(id);
                await _bookCommandService.Handle(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
