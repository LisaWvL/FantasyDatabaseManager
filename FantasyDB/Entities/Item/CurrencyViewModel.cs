using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities
{
    public class CurrencyViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int? Crown { get; set; } = 1;
        public int? Shilling { get; set; } = 12;
        public int? Penny { get; set; } = 240;
    }
}
