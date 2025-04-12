using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Interfaces;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class ChapterViewModel : IViewModelWithId
    {

        public int Id { get; set; }
        public int? ChapterNumber { get; set; } = 0;
        public string? ChapterText { get; set; } = string.Empty;
        public string? ChapterTitle { get; set; } = string.Empty;
        public string? ChapterSummary { get; set; } = string.Empty;
        public int? WordCount { get; set; } = 0;

        public string? ToDo { get; set; } = string.Empty;

        public int ActId { get; set; }
        public string? ActTitle { get; set; } = string.Empty;// Readable Name
        public int? ActNumber { get; set; } = 0;

        public int? POVCharacterId { get; set; } = 0;
        public string? POVCharacterName { get; set; } = string.Empty;// Readable Name

        public List<SceneViewModel>? Scenes { get; set; } = [];

    }
}
