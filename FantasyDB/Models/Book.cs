using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyDB.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int? BookNumber { get; set; } = 0;
        public string? SeriesTitle { get; set; } = string.Empty;
        public string? BookTitle { get; set; } = string.Empty;
        public int? BookWordCount { get; set; } = 0;
        public string? BookSummary { get; set; } = string.Empty;
        public string? BookToDo { get; set; } = string.Empty;

        public virtual ICollection<Act> Acts { get; set; } = [];
    }

}
