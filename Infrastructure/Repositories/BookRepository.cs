using BookManager.Core.Models;
using BookManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();

        public Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Book>>(_books);
        }

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var book = _books.SingleOrDefault(b => b.Id == id);
            return Task.FromResult(book);
        }

        public Task<IEnumerable<Book>> SearchAsync(string term, CancellationToken cancellationToken = default)
        {
            var results = _books
                .Where(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase)
                         || b.Author.Contains(term, StringComparison.OrdinalIgnoreCase)
                         || b.ISBN.Contains(term, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult<IEnumerable<Book>>(results);
        }

        public Task AddAsync(Book book, CancellationToken cancellationToken = default)
        {
            _books.Add(book);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            var existing = _books.SingleOrDefault(b => b.Id == book.Id);
            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.YearPublished = book.YearPublished;
                existing.ISBN = book.ISBN;
                existing.AvailableCopies = book.AvailableCopies;
            }
            return Task.CompletedTask;
        }
    }
}