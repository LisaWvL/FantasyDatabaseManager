using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Origin { get; set; }
        public string? Effects { get; set; }
        public int? OwnerId { get; set; }
        public int? ChapterId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("OwnerId")]
        public virtual Character? Owner { get; set; }
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }
    }
}
