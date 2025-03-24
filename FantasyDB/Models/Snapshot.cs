using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Snapshot
    {
        public int Id { get; set; }
        public string Book { get; set; } = string.Empty;
        public string? Act { get; set; } = string.Empty;
        public string? Chapter { get; set; } = string.Empty;
        public string? SnapshotName { get; set; } = string.Empty; // ✅ now loaded from DB

    }
}