using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Attributes;
using FantasyDB.Interfaces;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class PlotPointViewModel : IViewModelWithId
    {
        public int Id { get; set; }

        [EditableForSnapshot]
        public string Title { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Description { get; set; }

        [EditableForSnapshot]
        public int? StartDateId { get; set; }
        [EditableForSnapshot]
        public int? EndDateId { get; set; }
        public string? StartDateName { get; set; }
        public string? EndDateName { get; set; }


        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;


        // Junction References

        [HandlesJunction("PlotPointRiver", "PlotPointId", "RiverId")]
        public List<int> RiverIds { get; set; } = new();
        public List<string> RiverNames { get; set; } = new();

        [HandlesJunction("PlotPointRoute", "PlotPointId", "RouteId")]
        public List<int> RouteIds { get; set; } = new();
        public List<string> RouteNames { get; set; } = new();

    }
}