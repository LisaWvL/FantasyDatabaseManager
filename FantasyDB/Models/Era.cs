using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Era
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? MagicSystem { get; set; }
        public string? MagicStatus { get; set; }
        public int? ChapterId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }
    }
}

