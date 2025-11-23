using BookManager.Core.Models;
using BookManager.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _service.GetAllBooksAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            await _service.AddBookAsync(book);
            return CreatedAtAction(nameof(GetAllBooks), new { id = book.Id }, book);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<Book>> SearchBooks([FromQuery] string term)
        {
            return await _service.SearchBooksAsync(term);
        }

        [HttpPost("{id}/borrow")]
        public async Task<IActionResult> BorrowBook(Guid id)
        {
            await _service.BorrowBookAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnBook(Guid id)
        {
            await _service.ReturnBookAsync(id);
            return NoContent();
        }
    }
}
