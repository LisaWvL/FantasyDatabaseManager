using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Services
{
    public class DropdownService(AppDbContext context) : IDropdownService
    {

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetFactionsAsync() =>
            await context.Factions.AsNoTracking()
                .Select(f => new SimpleItem(f.Id, f.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetCharactersAsync() =>
            await context.Characters.AsNoTracking()
                .Select(c => new SimpleItem(c.Id, c.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetLocationsAsync() =>
            await context.Locations.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetLanguagesAsync() =>
            await context.Languages.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetBooksAsync() =>
            await context.Languages.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetActsAsync() =>
            await context.Languages.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetScenesAsync() =>
            await context.Languages.AsNoTracking()
                .Select(l => new SimpleItem(l.Id, l.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetChaptersAsync() =>
            await context.Chapters.AsNoTracking()
                .Select(s => new SimpleItem(s.Id, s.ChapterTitle))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetEventsAsync() =>
            await context.Events.AsNoTracking()
                .Select(e => new SimpleItem(e.Id, e.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetErasAsync() =>
            await context.Eras.AsNoTracking()
                .Select(s => new SimpleItem(s.Id, s.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetItemsAsync() =>
            await context.Items.AsNoTracking()
                .Select(a => new SimpleItem(a.Id, a.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.


        public async Task<List<SimpleItem>> GetCharacterRelationshipsForCharacterAsync(int characterId)
        {
            return await context.CharacterRelationships.AsNoTracking()
                .Where(cr => cr.Character1Id == characterId || cr.Character2Id == characterId)
                .Select(cr => new SimpleItem(
                    cr.Id,
                    cr.Character1!.Name + " - " + cr.RelationshipType + " - " + cr.Character2!.Name
                ))
                .ToListAsync();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetRoutesAsync() =>
            await context.Routes.AsNoTracking()
                .Select(r => new SimpleItem(r.Id, r.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<List<SimpleItem>> GetRiversAsync() =>
            await context.Rivers.AsNoTracking()
                .Select(r => new SimpleItem(r.Id, r.Name))
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.


        public async Task<List<SimpleItem>> GetWeekdaysAsync()
        {
            var weekdays = await context.Dates
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.Weekday))
                .Select(c => c.Weekday!)
                .Distinct()
                .OrderBy(w => w)
                .ToListAsync(); // <-- force query to execute here

            return [.. weekdays.Select((weekday, index) => new SimpleItem(index + 1, weekday))];
        }

        public async Task<List<SimpleItem>> GetMonthsAsync()
        {
            var months = await context.Dates
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.Month))
                .Select(c => c.Month!)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            return [.. months.Select((month, index) => new SimpleItem(index + 1, month))];
        }

        public async Task<List<SimpleItem>> GetCharacterRelationshipsAsync() =>
            await context.CharacterRelationships.AsNoTracking()
                .Select(r => new SimpleItem(
                    r.Id,
                    (r.Character1 != null ? r.Character1.Name : "??")
                    + " " + (r.RelationshipType ?? "?")
                    + " " + (r.Character2 != null ? r.Character2.Name : "??")
                ))
                .ToListAsync();

        public async Task<List<SimpleItem>> GetCalendarsAsync() =>
    await context.Dates.AsNoTracking()
        .Select(c => new SimpleItem(c.Id, $"Day {c.Day} - {c.Month} ({c.Weekday})"))
        .ToListAsync();

        public async Task<List<SimpleItem>> GetPriceExamplesAsync() =>
    await context.PriceExamples.AsNoTracking()
        .Select(p => new SimpleItem(p.Id, p.Name ?? $"Unnamed ({p.Category})"))
        .ToListAsync();

        public async Task<List<SimpleItem>> GetPlotPointsAsync() =>
    await context.PlotPoints.AsNoTracking()
        .Select(p => new SimpleItem(p.Id, p.Title))
        .ToListAsync();

    }
    public record SimpleItem(int Id, string Name);
}
