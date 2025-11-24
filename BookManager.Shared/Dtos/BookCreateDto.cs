using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager.Shared.Dtos
{
    public class BookCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Range(-8000, 2050)]
        public int YearPublished { get; set; }
        
        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }
    }
}
