using learning_center_back.Shared.Domain;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_back.Tutorials.Interfaces.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookQueryService _bookQueryService;
        private IBookCommandService _bookCommandService;
        public BookController(IBookQueryService bookQueryService, IBookCommandService bookCommandService)
        {
            _bookQueryService = bookQueryService;
            _bookCommandService = bookCommandService;
        }
        
        
        // GET: api/<BookController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var query = new GetAllBooksQuery();
            var result = await   _bookQueryService.Handler(query);
            
            if (result.Count() == 0 )  return BadRequest();
            
            return Ok(result);
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var query = new GetBookByIdQuery(id);
                
                if (id  == 0) return BadRequest();

                var result = await _bookQueryService.Handler(query);
                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        // POST api/<BookController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
