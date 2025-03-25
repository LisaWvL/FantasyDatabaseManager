using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Artifact
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Origin { get; set; }
        public string? Effects { get; set; }
        public int? OwnerId { get; set; }
        public int? SnapshotId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("OwnerId")]
        public virtual Character? Owner { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
    }
}
