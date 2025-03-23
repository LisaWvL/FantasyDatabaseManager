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
    public class EventViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Description { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? Day { get; set; }
        [EditableForSnapshot]
        public string? Month { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? Year { get; set; }
        public string? Purpose { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        [EditableForSnapshot]
        public int? LocationId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;// Readable Name
        public string? LocationName { get; set; } = string.Empty;// Readable Name
    }
}
