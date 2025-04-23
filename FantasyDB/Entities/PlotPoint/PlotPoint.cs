using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;

namespace FantasyDB.Entities
{
    public class PlotPoint
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? StartDateId { get; set; }
        [ForeignKey("StartDateId")]
        public virtual Date? StartDate { get; set; }

        public int? EndDateId { get; set; }
        [ForeignKey("EndDateId")]
        public virtual Date? EndDate { get; set; }

        //public List<Event> Events { get; set; } = [];

        // Junctions
        public List<PlotPointRiver> PlotPointRivers { get; set; } = [];
        public List<PlotPointRoute> PlotPointRoutes { get; set; } = [];
        public ICollection<ChapterPlotPoint> PlotPointChapters { get; set; } = new List<ChapterPlotPoint>();


    }

}