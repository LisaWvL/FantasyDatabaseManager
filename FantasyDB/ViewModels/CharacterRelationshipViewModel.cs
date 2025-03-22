using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class CharacterRelationshipViewModel
    {
        public string? Character1Name { get; set; } = string.Empty;// Readable Name
        public string? Character2Name { get; set; } = string.Empty;// Readable Name
        public string? RelationshipType { get; set; } = "Friend, Family, Ally, Sibling, Father, Mother, Mentor, Enemy, Archenemy";
        public string? RelationshipDynamic { get; set; } = string.Empty;
        public string? SnapshotName {get;set;} = string.Empty;
    }
}
