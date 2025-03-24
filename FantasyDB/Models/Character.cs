using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using FantasyDB.Models;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Character
    {
        public int Id { get; set; }
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public int? BirthDay { get; set; }
        public string? BirthMonth { get; set; }
        public int? BirthYear { get; set; }
        public string? Gender { get; set; }
        public int? HeightCm { get; set; }
        public string? Build { get; set; }
        public string? Hair { get; set; }
        public string? Eyes { get; set; }
        public string? DefiningFeatures { get; set; }
        public string? Personality { get; set; }
        public string? SocialStatus { get; set; }
        public string? Occupation { get; set; }
        public string? Magic { get; set; }
        public string? Desire { get; set; }
        public string? Fear { get; set; }
        public string? Weakness { get; set; }
        public string? Motivation { get; set; }
        public string? Flaw { get; set; }
        public string? Misbelief { get; set; }
        public int? SnapshotId { get; set; }
        public int? FactionId { get; set; }
        public int? LocationId { get; set; }
        public int? LanguageId { get; set; }

        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("FactionId")]
        public virtual Faction? Faction { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language? Language { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
    }

}