using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
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
        public string? ActTitle { get; set; } = string.Empty;
        public int? ActNumber { get; set; } = 0;

        public int? POVCharacterId { get; set; } = 0;
        public string? POVCharacterName { get; set; } = string.Empty;

        public int? StartDateId { get; set; } = 0;
        public int? EndDateId { get; set; } = 0;
        public List<int> SceneIds { get; set; } = [];
        public List<SceneViewModel>? Scenes { get; set; } = [];
        public List<int>? ChapterIds { get; set; } = [];

        public List<string>? ChapterTitles { get; set; } = [];

    }

}
