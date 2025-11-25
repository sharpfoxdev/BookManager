using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Core.Models
{
    public class LoanHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // borrow or return book
        public string Action { get; set; } = string.Empty;
        public Guid BookId { get; set; }
        public Book? Book { get; set; }
    }
}
