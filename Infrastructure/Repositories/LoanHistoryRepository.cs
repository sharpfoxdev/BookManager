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
    public class LoanHistoryRepository : ILoanHistoryRepository
    {
        private readonly BookManagerDbContext _context;

        public LoanHistoryRepository(BookManagerDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoanHistory history, CancellationToken cancellationToken = default)
        {
            await _context.LoanHistories.AddAsync(history, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<LoanHistory>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
        {
            return await _context.LoanHistories
                .Where(x => x.BookId == bookId)
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync(cancellationToken);
        }
    }
}
