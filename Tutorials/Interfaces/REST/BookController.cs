using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Shared.Infraestructure.Attribute;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Exceptions;
using learning_center_back.Tutorials.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;

namespace learning_center_back.Tutorials.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BookController(IBookQueryService bookQueryService, IBookCommandService bookCommandService) : ControllerBase
    {
        private readonly IBookQueryService _bookQueryService = bookQueryService;
        private readonly IBookCommandService _bookCommandService = bookCommandService;

        /// <summary>
        /// Obtain all active the books in th system with chapters whithout filters.
        /// </summary>
        // GET: api/Book
        [HttpGet]
        [CustomAuthorize("admin,sales")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _bookQueryService.Handle(new GetAllBooksQuery());
            return result.Any() ? Ok(result.Select(BookResourceFromEntityAssembler.ToResourceFromEntity)) : NotFound("No books found.");
        }
        
        /// <summary>
        /// Obtain active the book with its chapters based filter by id.
        /// </summary>
        /// <param name="id"></param>
        // GET: api/Book/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            var result = await _bookQueryService.Handle(new GetBookByIdQuery(id));
            return result != null ? Ok(BookResourceFromEntityAssembler.ToResourceFromEntity(result)) : NotFound($"Book with ID {id} not found.");
        }

        
        
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/Book
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "Description": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="407">The value are not expected</response>
        // POST: api/Book
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        [CustomAuthorize("admin")]
        public async Task<IActionResult> Post([FromBody] CreateBookCommand command)
        {
            if (command == null) return BadRequest("Book name cannot be empty.");

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
            catch (NotChapterFoundException ex)
            {
                return BadRequest(ex.Message);
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
        [CustomAuthorize("admin,sales")]
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
