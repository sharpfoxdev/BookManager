using BookManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Core.Repositories
{
    public interface ILoanHistoryRepository
    {
        Task AddAsync(LoanHistory history, CancellationToken cancellationToken = default);
        Task<List<LoanHistory>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
    }
}
