using AutoMapper;
using BookManager.Core.Models;
using BookManager.Core.Services;
using BookManager.Infrastructure.Services;
using BookManager.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookService service, IMapper mapper)
        {
            _bookService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks(CancellationToken cancellationToken = default)
        {
            var books = await _bookService.GetAllBooksAsync(cancellationToken);
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(bookDtos);
        }
        //[HttpGet]
        //public async Task<IEnumerable<Book>> GetAllBooks()
        //{
        //    return await _bookService.GetAllBooksAsync();
        //}
        [HttpPost]
        public async Task<IActionResult> AddBook(BookCreateDto createDto, CancellationToken cancellationToken = default)
        {
            var book = _mapper.Map<Book>(createDto);
            // maybe set book.Id = Guid.NewGuid();
            await _bookService.AddBookAsync(book, cancellationToken);
            var responseDto = _mapper.Map<BookDto>(book);
            return CreatedAtAction(nameof(GetBookById), new { id = responseDto.Id }, responseDto);
        }
        //[HttpPost]
        //public async Task<IActionResult> AddBook(Book book)
        //{
        //    await _bookService.AddBookAsync(book);
        //    return CreatedAtAction(nameof(GetAllBooks), new { id = book.Id }, book);
        //}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookDto>> GetBookById(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByIdAsync(id, cancellationToken);
            if (book is null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] string term, CancellationToken cancellationToken = default)
        {
            var results = await _bookService.SearchBooksAsync(term, cancellationToken);
            var dtoResults = _mapper.Map<IEnumerable<BookDto>>(results);
            return Ok(dtoResults);
        }
        //[HttpGet("search")]
        //public async Task<IEnumerable<Book>> SearchBooks([FromQuery] string term)
        //{
        //    return await _bookService.SearchBooksAsync(term);
        //}
        [HttpPost("{id:guid}/borrow")]
        public async Task<IActionResult> BorrowBook(Guid id, CancellationToken cancellationToken = default)
        {
            await _bookService.BorrowBookAsync(id, cancellationToken);
            return NoContent();
        }
        //[HttpPost("{id}/borrow")]
        //public async Task<IActionResult> BorrowBook(Guid id)
        //{
        //    await _bookService.BorrowBookAsync(id);
        //    return NoContent();
        //}

        [HttpPost("{id:guid}/return")]
        public async Task<IActionResult> ReturnBook(Guid id, CancellationToken cancellationToken = default)
        {
            await _bookService.ReturnBookAsync(id, cancellationToken);
            return NoContent();
        }
        //[HttpPost("{id}/return")]
        //public async Task<IActionResult> ReturnBook(Guid id)
        //{
        //    await _bookService.ReturnBookAsync(id);
        //    return NoContent();
        //}
    }
}
