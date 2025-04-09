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
        [ForeignKey("startDateId")]
        public virtual Calendar? StartDate { get; set; }

        public int? endDateId { get; set; }
        [ForeignKey("endDateId")]
        public virtual Calendar? endDate { get; set; }

        public int? ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }

        // Junctions
        public List<PlotPointRiver> PlotPointRivers { get; set; } = new();
        public List<PlotPointRoute> PlotPointRoutes { get; set; } = new();

    }
}