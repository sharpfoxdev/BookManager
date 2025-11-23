using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Core.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public int YearPublished { get; set; }
        public string ISBN { get; set; } = default!;
        public int AvailableCopies { get; set; }
    }
}
