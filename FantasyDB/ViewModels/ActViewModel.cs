using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Interfaces;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class ActViewModel : IViewModelWithId
    {
        public int Id { get; set; }

        public string? ActTitle { get; set; } = string.Empty;
        public int? ActNumber { get; set; } = 0;
        public string? ActSummary { get; set; } = string.Empty;
        public string? ActToDo { get; set; } = string.Empty;
        public int? ActWordCount { get; set; } = 0;

        public int? BookId { get; set; } = 0;
        public string? BookTitle { get; set; } = string.Empty;// Readable Name
        public int? BookNumber { get; set; } = 0;
        public List<ChapterViewModel>? Chapters { get; set; } = [];
    }
}
