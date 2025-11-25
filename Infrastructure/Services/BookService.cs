using BookManager.Core.Models;
using BookManager.Core.Repositories;
using BookManager.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default)
        {
            return _repo.GetAllAsync(cancellationToken);
        }

        public Task<Book?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _repo.GetByIdAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Book>> SearchBooksAsync(string term, CancellationToken cancellationToken = default)
        {
            return _repo.SearchAsync(term, cancellationToken);
        }

        public async Task AddBookAsync(Book book, CancellationToken cancellationToken = default)
        {
            await _repo.AddAsync(book, cancellationToken);
        }

        public async Task BorrowBookAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _repo.GetByIdAsync(id, cancellationToken);
            if (book == null) throw new InvalidOperationException("Book not found");
            if (book.AvailableCopies <= 0) throw new InvalidOperationException("No copies available");
            book.AvailableCopies--;
            book.VersionToken = Guid.NewGuid();

            try
            {
                await _repo.UpdateAsync(book, cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("The book record was modified concurrently. Please reload and try again.");
            }
        }

        public async Task ReturnBookAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _repo.GetByIdAsync(id, cancellationToken);
            if (book == null) throw new InvalidOperationException("Book not found");
            if (book.AvailableCopies == int.MaxValue)
                throw new InvalidOperationException("AvailableCopies overflow detected");
            book.AvailableCopies++;
            book.VersionToken = Guid.NewGuid();
            try
            {
                await _repo.UpdateAsync(book, cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("The book record was modified concurrently. Please reload and try again.");
            }
        }
    }
}