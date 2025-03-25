using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class PriceExampleViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Category { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Exclusivity { get; set; } = string.Empty;
        public int? Price { get; set; }
    }
}
