using System.Collections.Generic;
using System.Threading.Tasks;



namespace FantasyDB.Entities._Shared
{
    public interface IDropdownService
    {
        Task<List<SimpleItem>> GetFactionsAsync();
        Task<List<SimpleItem>> GetCharactersAsync();
        Task<List<SimpleItem>> GetLocationsAsync();
        Task<List<SimpleItem>> GetLanguagesAsync();

        Task<List<SimpleItem>> GetBooksAsync();
        Task<List<SimpleItem>> GetActsAsync();
        Task<List<SimpleItem>> GetScenesAsync();
        Task<List<SimpleItem>> GetChaptersAsync();

        Task<List<SimpleItem>> GetEventsAsync();
        Task<List<SimpleItem>> GetErasAsync();
        Task<List<SimpleItem>> GetItemsAsync();
        Task<List<SimpleItem>> GetRoutesAsync();
        Task<List<SimpleItem>> GetRiversAsync();
        Task<List<SimpleItem>> GetWeekdaysAsync();
        Task<List<SimpleItem>> GetMonthsAsync();
        Task<List<SimpleItem>> GetCharacterRelationshipsForCharacterAsync(int characterId);
        Task<List<SimpleItem>> GetCharacterRelationshipsAsync();
        Task<List<SimpleItem>> GetPriceExamplesAsync();
        Task<List<SimpleItem>> GetPlotPointsAsync();
        Task<List<SimpleItem>> GetDatesAsync();


    }
}
