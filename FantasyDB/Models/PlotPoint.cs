using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Models
{
    public class PlotPoint
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? startDateId { get; set; }
        public virtual Calendar? StartDate { get; set; }

        public int? endDateId { get; set; }
        public virtual Calendar? endDate { get; set; }

        public int? SnapshotId { get; set; }
        public virtual Snapshot? Snapshot { get; set; }

        // Junctions
        public List<PlotPointRiver> PlotPointRivers { get; set; } = new();
        public List<PlotPointRoute> PlotPointRoutes { get; set; } = new();

    }
}