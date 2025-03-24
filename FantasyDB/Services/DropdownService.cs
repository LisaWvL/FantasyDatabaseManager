using Microsoft.AspNetCore.Mvc.Rendering;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace FantasyDB.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly AppDbContext _context;

        public DropdownService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetFactionsAsync() =>
            await _context.Faction.AsNoTracking()
                .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetCharactersAsync() =>
            await _context.Character.AsNoTracking()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetLocationsAsync() =>
            await _context.Location.AsNoTracking()
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToListAsync();


        public async Task<List<SelectListItem>> GetChildLocationsAsync() =>
            await _context.Location
                .AsNoTracking()
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                }).ToListAsync();

        public async Task<List<SelectListItem>> GetLanguagesAsync() =>
            await _context.Language.AsNoTracking()
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetSnapshotsAsync() =>
            await _context.Snapshot.AsNoTracking()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SnapshotName })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetEventsAsync() =>
            await _context.Event
                .AsNoTracking()
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                }).ToListAsync();

        public async Task<List<SelectListItem>> GetErasAsync() =>
            await _context.Era.AsNoTracking()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToListAsync();

        

        // Implement the rest of the interface methods here, based on your actual data model
        public async Task<List<SelectListItem>> GetRoutesAsync() =>
            await _context.Route.AsNoTracking()
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetRiversAsync() =>
            await _context.River.AsNoTracking()
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                .ToListAsync();

 

        private Dictionary<string, List<SelectListItem>> _cache = new();

        public async Task LoadDropdownsByKeys(ViewDataDictionary viewData, IEnumerable<string> keys)
        {
            var loaded = new Dictionary<string, List<SelectListItem>>();

            foreach (var key in keys.Distinct())
            {
                var viewDataKey = $"{key}List";
                if (viewData.ContainsKey(viewDataKey))
                    continue;

                switch (key)
                {
                    case "FactionId":
                        if (!loaded.ContainsKey("Factions"))
                            loaded["Factions"] = await GetFactionsAsync();
                        viewData[viewDataKey] = loaded["Factions"];
                        break;

                    case "LanguageId":
                        if (!loaded.ContainsKey("Languages"))
                            loaded["Languages"] = await GetLanguagesAsync();
                        viewData[viewDataKey] = loaded["Languages"];
                        break;

                    case "LocationId":
                    case "HQLocationId":
                    case "ParentLocationId":
                    case "ChildLocationsId":
                    case "SourceLocationId":
                    case "DestinationLocationId":
                    case "FromId":
                    case "ToId":
                        if (!loaded.ContainsKey("Locations"))
                            loaded["Locations"] = await GetLocationsAsync();
                        viewData[viewDataKey] = loaded["Locations"];
                        break;

                    case "SnapshotId":
                        if (!loaded.ContainsKey("Snapshots"))
                            loaded["Snapshots"] = await GetSnapshotsAsync();
                        viewData[viewDataKey] = loaded["Snapshots"];
                        break;

                    case "CharacterId":
                    case "Character1Id":
                    case "Character2Id":
                    case "FounderId":
                    case "LeaderId":
                    case "OwnerId":
                        if (!loaded.ContainsKey("Characters"))
                            loaded["Characters"] = await GetCharactersAsync();
                        viewData[viewDataKey] = loaded["Characters"];
                        break;

                    case "EventId":
                        if (!loaded.ContainsKey("Events"))
                            loaded["Events"] = await GetEventsAsync();
                        viewData[viewDataKey] = loaded["Events"];
                        break;

                    case "EventIds":
                        if (!loaded.ContainsKey("Events"))
                            loaded["Events"] = await GetEventsAsync();
                        viewData[viewDataKey] = loaded["Events"];
                        break;

                    case "EraId":
                        if (!loaded.ContainsKey("Eras"))
                            loaded["Eras"] = await GetErasAsync();
                        viewData[viewDataKey] = loaded["Eras"];
                        break;

                    case "RouteId":
                        if (!loaded.ContainsKey("Routes"))
                            loaded["Routes"] = await GetRoutesAsync();
                        viewData[viewDataKey] = loaded["Routes"];
                        break;

                    case "RiverId":
                        if (!loaded.ContainsKey("Rivers"))
                            loaded["Rivers"] = await GetRiversAsync();
                        viewData[viewDataKey] = loaded["Rivers"];
                        break;


                }
            }
        }

        public async Task LoadDropdownsForViewModel<TViewModel>(ViewDataDictionary viewData)
        {
            var dropdownKeys = typeof(TViewModel).GetProperties()
                .Where(p => p.Name.EndsWith("Id") && p.PropertyType == typeof(int?))
                .Select(p => p.Name)
                .Distinct();

            await LoadDropdownsByKeys(viewData, dropdownKeys);
        }

    }
}
        
    