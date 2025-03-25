using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class CharacterViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        [EditableForSnapshot]
        public string? Role { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Alias { get; set; } = string.Empty;
        public int? BirthDay { get; set; }
        public string? BirthMonth { get; set; } = string.Empty;
        public int? BirthYear { get; set; }
        public string? Gender { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? HeightCm { get; set; }
        [EditableForSnapshot]
        public string? Build { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Hair { get; set; } = string.Empty;
        public string? Eyes { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? DefiningFeatures { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Personality { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? SocialStatus { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Occupation { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Magic { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Desire { get; set; } = string.Empty;
        [EditableForSnapshot]

        public string? Fear { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Weakness { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Motivation { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Flaw { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Misbelief { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        [EditableForSnapshot]
        public int? FactionId { get; set; }
        [EditableForSnapshot]
        public int? LocationId { get; set; }
        [EditableForSnapshot]
        public int? LanguageId { get; set; }

        public string? FactionName { get; set; } = string.Empty;// Readable Name
        public string? LocationName { get; set; } = string.Empty;// Readable Name
        public string? LanguageName { get; set; } = string.Empty;// Readable Name
        public string? SnapshotName { get; set; } = string.Empty;//Readable Name
    }
}
