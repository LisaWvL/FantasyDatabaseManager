using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class ArtifactViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Origin { get; set; } = string.Empty;
        public string? Effects { get; set; } = string.Empty;
        [NotMapped]
        public string? OwnerName { get; set; } = string.Empty; // Readable Name
        [NotMapped]
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
