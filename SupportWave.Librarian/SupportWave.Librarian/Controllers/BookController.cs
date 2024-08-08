using Microsoft.AspNetCore.Mvc;
using SupportWave.Librarian.Api.Models;
using SupportWave.Librarian.Api.Services.Books;
using SupportWave.Librarian.Api.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportWave.Librarian.Api.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            this._bookService = bookService;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Json(this._bookService.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return Json(new LibrarianException(System.Net.HttpStatusCode.NotFound, "Invalid id provided", "Provide a valid id", id));
            }

            var result = this._bookService.Get(id);
            return result is null? Json(new LibrarianException(System.Net.HttpStatusCode.NotFound, "Book does not exist", null, id)) : Json(result);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BookModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await this._bookService.Insert(value);

            return result is null ? 
                Json(new LibrarianException(System.Net.HttpStatusCode.OK, "Failed to save book", "Please check list and try again", value)) : 
                Json(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]BookModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == Guid.Empty)
            {
                return Json(new LibrarianException(System.Net.HttpStatusCode.NotFound, "Invalid id provided", "Provide a valid id", id));
            }
            
            var result = await this._bookService.Update(id, value);

            return result is null ? 
                Json(new LibrarianException(System.Net.HttpStatusCode.OK, "Failed to update book", "Please check list and try again", value)) : 
                Json(result);           
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var deleted = await this._bookService.Delete(id);

            return deleted ? Ok() : 
                Json(new LibrarianException(System.Net.HttpStatusCode.BadRequest,
                "Error deleting book, please try again", 
                "Check if book exists and try again", 
                id));
        }
    }
}

