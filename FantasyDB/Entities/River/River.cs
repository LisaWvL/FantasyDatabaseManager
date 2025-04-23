using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities // ✅ Add this line
{
    public class River
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int? DepthMeters { get; set; }
        public int? WidthMeters { get; set; }
        public string? FlowDirection { get; set; }
        public int? SourceLocationId { get; set; }
        public int? DestinationLocationId { get; set; }
        // ✅ Navigation Properties (Add These Back)

        [ForeignKey("SourceLocationId")]
        public virtual Location? SourceLocation { get; set; }
        [ForeignKey("DestinationLocationId")]
        public virtual Location? DestinationLocation { get; set; }

    }
}