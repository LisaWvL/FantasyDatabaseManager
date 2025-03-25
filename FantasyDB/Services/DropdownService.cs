using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly AppDbContext _context;

        public DropdownService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SimpleItem>> GetFactionsAsync() =>
            await _context.Factions.AsNoTracking()
                .Select(f => new SimpleItem(f.Id, f.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetCharactersAsync() =>
            await _context.Characters.AsNoTracking()
                .Select(c => new SimpleItem(c.Id, c.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetLocationsAsync() =>
            await _context.Locations.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetLanguagesAsync() =>
            await _context.Languages.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetSnapshotsAsync() =>
            await _context.Snapshots.AsNoTracking()
                .Select(s => new SimpleItem(s.Id, s.SnapshotName))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetEventsAsync() =>
            await _context.Events.AsNoTracking()
                .Select(e => new SimpleItem(e.Id, e.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetErasAsync() =>
            await _context.Eras.AsNoTracking()
                .Select(s => new SimpleItem(s.Id, s.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetArtifactsAsync() =>
            await _context.Artifacts.AsNoTracking()
                .Select(a => new SimpleItem(a.Id, a.Name))
                .ToListAsync();


        public async Task<List<SimpleItem>> GetCharacterRelationshipsForCharacterAsync(int characterId)
        {
            return await _context.CharacterRelationships.AsNoTracking()
                .Where(cr => cr.Character1Id == characterId || cr.Character2Id == characterId)
                .Select(cr => new SimpleItem(
                    cr.Id,
                    cr.Character1!.Name + " - " + cr.RelationshipType + " - " + cr.Character2!.Name
                ))
                .ToListAsync();
        }

        public async Task<List<SimpleItem>> GetRoutesAsync() =>
            await _context.Routes.AsNoTracking()
                .Select(r => new SimpleItem(r.Id, r.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetRiversAsync() =>
            await _context.Rivers.AsNoTracking()
                .Select(r => new SimpleItem(r.Id, r.Name))
                .ToListAsync();


        public async Task<List<SimpleItem>> GetMonthsAsync()
        {
            return await _context.Calendar
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.Month))
                .Select(c => c.Month!)
                .Distinct()
                .OrderBy(m => m) // Optional: alphabetically or keep a fixed order if needed
                .Select((month, index) => new SimpleItem(index + 1, month))
                .ToListAsync();
        }

        public async Task<List<SimpleItem>> GetWeekdaysAsync()
        {
            return await _context.Calendar
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.Weekday))
                .Select(c => c.Weekday!)
                .Distinct()
                .OrderBy(w => w)
                .Select((weekday, index) => new SimpleItem(index + 1, weekday))
                .ToListAsync();
        }

        public async Task<List<SimpleItem>> GetCharacterRelationshipsAsync() =>
            await _context.CharacterRelationships.AsNoTracking()
                .Select(r => new SimpleItem(
                    r.Id,
                    (r.Character1 != null ? r.Character1.Name : "??")
                    + " " + (r.RelationshipType ?? "?")
                    + " " + (r.Character2 != null ? r.Character2.Name : "??")
                ))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetCalendarsAsync() =>
    await _context.Calendar.AsNoTracking()
        .Select(c => new SimpleItem(c.Id, $"Day {c.Day} - {c.Month} ({c.Weekday})"))
        .ToListAsync();

        public async Task<List<SimpleItem>> GetPriceExamplesAsync() =>
    await _context.PriceExamples.AsNoTracking()
        .Select(p => new SimpleItem(p.Id, p.Name ?? $"Unnamed ({p.Category})"))
        .ToListAsync();

        public async Task<List<SimpleItem>> GetPlotPointsAsync() =>
    await _context.PlotPoints.AsNoTracking()
        .Select(p => new SimpleItem(p.Id, p.Title))
        .ToListAsync();

    }
    public record SimpleItem(int Id, string Name);
}
