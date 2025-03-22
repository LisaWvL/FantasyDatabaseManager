using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantasyDB.Services
{
    public interface IDropdownService
    {
        Task<List<SelectListItem>> GetFactionsAsync();
        Task<List<SelectListItem>> GetCharactersAsync();
        Task<List<SelectListItem>> GetLocationsAsync();
        Task<List<SelectListItem>> GetLanguagesAsync();
        Task<List<SelectListItem>> GetSnapshotsAsync();
        Task<List<SelectListItem>> GetEventsAsync();
        Task<List<SelectListItem>> GetErasAsync();

        Task LoadDropdowns(ViewDataDictionary viewData); // ✅ Add this!
    }
}



