using BookManager.Core.Models;
using BookManager.Core.Repositories;
using BookManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookManagerDbContext _db;

        public BookRepository(BookManagerDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Books.ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Books.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<Book>> SearchAsync(string term, CancellationToken cancellationToken = default)
        {
            return await _db.Books
                .Where(b => b.Title.Contains(term)
                         || b.Author.Contains(term)
                         || b.ISBN.Contains(term))
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}