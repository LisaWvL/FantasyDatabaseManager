using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class CharacterViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        [EditableForChapter]
        public string? Role { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Alias { get; set; } = string.Empty;
        public int? BirthDay { get; set; }
        public string? BirthMonth { get; set; } = string.Empty;
        public int? BirthYear { get; set; }
        public string? Gender { get; set; } = string.Empty;
        [EditableForChapter]
        public int? HeightCm { get; set; }
        [EditableForChapter]
        public string? Build { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Hair { get; set; } = string.Empty;
        public string? Eyes { get; set; } = string.Empty;
        [EditableForChapter]
        public string? DefiningFeatures { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Personality { get; set; } = string.Empty;
        [EditableForChapter]
        public string? SocialStatus { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Occupation { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Magic { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Desire { get; set; } = string.Empty;
        [EditableForChapter]

        public string? Fear { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Weakness { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Motivation { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Flaw { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Misbelief { get; set; } = string.Empty;
        [EditableForChapter]
        public int? ChapterId { get; set; }
        [EditableForChapter]
        public int? FactionId { get; set; }
        [EditableForChapter]
        public int? LocationId { get; set; }
        [EditableForChapter]
        public int? LanguageId { get; set; }

        public string? FactionName { get; set; } = string.Empty;// Readable Name
        public string? LocationName { get; set; } = string.Empty;// Readable Name
        public string? LanguageName { get; set; } = string.Empty;// Readable Name
        public int? ChapterNumber { get; set; } = 0;//Readable Name
    }
}
