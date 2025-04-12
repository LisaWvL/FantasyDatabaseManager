using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Interfaces;
using FantasyDB.Attributes;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class ConversationTurnViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Prompt { get; set; } = string.Empty;
        public string? Response { get; set; } = string.Empty;
        public bool? DanMode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int? PlotPointId { get; set; }
        public bool IsSummary { get; set; } = false;
        public string? Role { get; set; } = string.Empty; // "system", "user", "assistant", or "summary"
        public int? TokenCount { get; set; }
        public int? SummaryLevel { get; set; } = 0; // hierarchical memory later (summary of summaries).
        public int? ParentId { get; set; }


    }
}
