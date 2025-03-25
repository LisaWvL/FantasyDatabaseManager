using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class RouteViewModel : IViewModelWithId
    {

        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public int? Length { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public string? TravelTime { get; set; } = string.Empty;


        public int? FromId { get; set; }
        public int? ToId { get; set; }


        public string? ToLocationName { get; set; } = string.Empty;
        public string? FromLocationName { get; set; } = string.Empty;

    }
}
