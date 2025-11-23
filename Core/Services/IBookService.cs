using BookManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Core.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Book>> SearchBooksAsync(string term, CancellationToken cancellationToken = default);
        Task AddBookAsync(Book book, CancellationToken cancellationToken = default);
        Task BorrowBookAsync(Guid id, CancellationToken cancellationToken = default);
        Task ReturnBookAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
