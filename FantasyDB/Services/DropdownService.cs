using Microsoft.AspNetCore.Mvc.Rendering;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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

        public async Task<List<SelectListItem>> GetLanguagesAsync() =>
            await _context.Language.AsNoTracking()
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Type })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetSnapshotsAsync() =>
            await _context.Snapshot.AsNoTracking()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SnapshotName })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetEventsAsync() =>
            await _context.Event.AsNoTracking()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToListAsync();

        public async Task<List<SelectListItem>> GetErasAsync() =>
            await _context.Era.AsNoTracking()
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToListAsync();

        public async Task LoadDropdowns(ViewDataDictionary viewData)
        {
            viewData["FactionIdList"] = await GetFactionsAsync();
            viewData["Character1IdList"] = await GetCharactersAsync();
            viewData["Character2IdList"] = await GetCharactersAsync(); // For CharacterRelationship
            viewData["LocationIdList"] = await GetLocationsAsync();
            viewData["LanguageIdList"] = await GetLanguagesAsync();
            viewData["SnapshotIdList"] = await GetSnapshotsAsync();
            viewData["EventIdList"] = await GetEventsAsync();
            viewData["EraIdList"] = await GetErasAsync();
 

        // New Additions
            viewData["Character1IdList"] = await GetCharactersAsync();
            viewData["Character2IdList"] = await GetCharactersAsync();
            viewData["FounderIdList"] = await GetCharactersAsync();
            viewData["LeaderIdList"] = await GetCharactersAsync();
            viewData["HQLocationIdList"] = await GetLocationsAsync();
            viewData["ParentLocationIdList"] = await GetLocationsAsync();
            viewData["ChildLocationIdList"] = await GetLocationsAsync();
            viewData["SourceLocationIdList"] = await GetLocationsAsync();
            viewData["DestinationLocationIdList"] = await GetLocationsAsync();
            viewData["ToLocationIdList"] = await GetLocationsAsync();
            viewData["FromLocationIdList"] = await GetLocationsAsync();
        }


    }
}
