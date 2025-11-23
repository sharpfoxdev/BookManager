using BookManager.Core.Models;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Core.Repositories
{

    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Book>> SearchAsync(string term, CancellationToken cancellationToken = default);
        Task AddAsync(Book book, CancellationToken cancellationToken = default);
        Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    }
    
}
