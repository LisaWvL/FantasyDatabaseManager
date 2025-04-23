using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;
using Humanizer;
using Microsoft.CodeAnalysis;

namespace FantasyDB.Entities
{
    public class ConversationTurn
    {
        public int Id { get; set; }
        public string? Prompt { get; set; } = string.Empty;
        public string? Response { get; set; } = string.Empty;
        public bool? DanMode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool? IsSummary { get; set; }
        public string? Role { get; set; } = string.Empty; // "system", "user", "assistant", or "summary"
        public int? TokenCount { get; set; } 
        public int? SummaryLevel { get; set; } = 0; // hierarchical memory later (summary of summaries).
        public int? ParentId { get; set; }


        public int? PlotPointId { get; set; }
        [ForeignKey("PlotPointId")]
        public PlotPoint? PlotPoint { get; set; }
    }
}
