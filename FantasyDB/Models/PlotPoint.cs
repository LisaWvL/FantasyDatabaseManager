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

        public int? StartDateId { get; set; }
        [ForeignKey("StartDateId")]
        public virtual Calendar? StartDate { get; set; }

        public int? EndDateId { get; set; }
        [ForeignKey("EndDateId")]
        public virtual Calendar? EndDate { get; set; }

        public int? ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }

        // Junctions
        public List<PlotPointRiver> PlotPointRivers { get; set; } = [];
        public List<PlotPointRoute> PlotPointRoutes { get; set; } = [];

    }
}