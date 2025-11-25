using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Shared.Dtos
{
    public class LoanHistoryDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
