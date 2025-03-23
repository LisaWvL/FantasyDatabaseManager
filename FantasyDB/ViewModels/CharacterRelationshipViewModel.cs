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
    public class CharacterRelationshipViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Character1Name { get; set; } = string.Empty;// Readable Name
        public string? Character2Name { get; set; } = string.Empty;// Readable Name
        [EditableForSnapshot]
        public string? RelationshipType { get; set; } = "Friend, Family, Ally, Sibling, Father, Mother, Mentor, Enemy, Archenemy";
        [EditableForSnapshot]
        public string? RelationshipDynamic { get; set; } = string.Empty;
        public int? Character1Id { get; set; }
        public int? Character2Id { get; set; }
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName {get;set;} = string.Empty;
    }
}
