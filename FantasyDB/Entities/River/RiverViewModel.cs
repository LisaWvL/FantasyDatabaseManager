using FantasyDB.Entities._Shared;
using FantasyDB.Entities;






namespace FantasyDB.Entities
{
    public class RiverViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int? DepthMeters { get; set; }
        public int? WidthMeters { get; set; }
        public string? FlowDirection { get; set; } = string.Empty;


        public int? SourceLocationId { get; set; }
        public int? DestinationLocationId { get; set; }


        public string? SourceLocationName { get; set; } = string.Empty;
        public string? DestinationLocationName { get; set; } = string.Empty;
    }
}
