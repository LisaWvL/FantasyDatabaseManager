using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities // ✅ Add this line
{

    public class Era
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? MagicSystem { get; set; }
        public string? MagicStatus { get; set; }
        public int? ChapterId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        public virtual Chapter? Chapter { get; set; }
        public int? StartDateId { get; set; } = 0;
        public virtual Date? StartDate { get; set; }
        public int? EndDateId { get; set; } = 0;
        public virtual Date? EndDate { get; set; }
    }

}

