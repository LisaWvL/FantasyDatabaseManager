using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using FantasyDB.Services;

namespace FantasyDB.ViewModels
{
    public class ArtifactViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Origin { get; set; } = string.Empty;
        public string? Effects { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        [EditableForSnapshot]
        public int? OwnerId { get; set; }
        public string? OwnerName { get; set; } = string.Empty; // Readable Name
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
