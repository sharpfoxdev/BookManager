using AutoMapper;
using BookManager.Core.Models;
using BookManager.Core.Repositories;
using BookManager.Core.Services;
using BookManager.Infrastructure.Services;
using BookManager.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManager.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILoanHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookService service, ILoanHistoryRepository historyRepository, IMapper mapper)
        {
            _bookService = service;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks(CancellationToken cancellationToken = default)
        {
            var books = await _bookService.GetAllBooksAsync(cancellationToken);
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(bookDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookCreateDto createDto, CancellationToken cancellationToken = default)
        {
            var book = _mapper.Map<Book>(createDto);
            await _bookService.AddBookAsync(book, cancellationToken);
            var responseDto = _mapper.Map<BookDto>(book);
            return CreatedAtAction(nameof(GetBookById), new { id = responseDto.Id }, responseDto);
        }

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

        [HttpPost("{id:guid}/borrow")]
        public async Task<IActionResult> BorrowBook(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _bookService.BorrowBookAsync(id, cancellationToken);
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("No copies available", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Book not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(new { ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("concurrently", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred." });
            }
        }

        [HttpPost("{id:guid}/return")]
        public async Task<IActionResult> ReturnBook(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _bookService.ReturnBookAsync(id, cancellationToken);
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("overflow", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Book not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(new { ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("concurrently", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred." });
            }
        }
        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<LoanHistoryDto>>> GetHistory(Guid id, CancellationToken cancellationToken)
        {
            var history = await _historyRepository.GetByBookIdAsync(id, cancellationToken);
            return Ok(_mapper.Map<List<LoanHistoryDto>>(history));
        }
    }
}
