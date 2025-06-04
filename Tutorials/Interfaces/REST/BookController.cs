using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_back.Tutorials.Interfaces.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookQueryService _bookQueryService;
        private readonly IBookCommandService _bookCommandService;

        public BookController(IBookQueryService bookQueryService, IBookCommandService bookCommandService)
        {
            _bookQueryService = bookQueryService ?? throw new ArgumentNullException(nameof(bookQueryService));
            _bookCommandService = bookCommandService ?? throw new ArgumentNullException(nameof(bookCommandService));
        }

        // GET: api/Book
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var query = new GetAllBooksQuery();
            var result = await _bookQueryService.Handler(query);

            return result.Any() ? Ok(result) : NotFound("No books found.");
        }

        // GET: api/Book/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            try
            {
                var query = new GetBookByIdQuery(id);
                var result = await _bookQueryService.Handler(query);

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

            try
            {
                await _bookCommandService.Handler(command);
                return StatusCode(StatusCodes.Status201Created);
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
        public async Task<IActionResult> Put(int id, [FromBody] string command)
        {
            return Ok();
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");

            try
            {
                var command = new DeleteBookCommand(id);
                await _bookCommandService.Handler(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
