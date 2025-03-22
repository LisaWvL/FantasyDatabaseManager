using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services; // Required for IDropdownService and BaseEntityController

namespace FantasyDBStartup.Controllers
{
    public class LocationController : Controller
    {
        private readonly AppDbContext _context;

        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var locations = await _context.Location
                .AsNoTracking()
                .ToListAsync();

            var viewModel = locations
                .Select(loc => new LocationViewModel
                {
                    Id = loc.Id,
                    Name = loc.Name,
                    Type = loc.Type,
                    Biome = loc.Biome,
                    Cultures = loc.Cultures,
                    Politics = loc.Politics,
                    TotalPopulation = loc.TotalPopulation,
                    DivineMagicians = loc.DivineMagicians,
                    WildMagicians = loc.WildMagicians,
                    ChildLocationId = loc.ChildLocationId,
                    ParentLocationId = loc.ParentLocationId,
                    EventId = loc.EventId,
                    LanguageId = loc.LanguageId,
                    SnapshotId = loc.SnapshotId,

                    ChildLocationName = _context.Location.FirstOrDefault(c => c.Id == loc.ChildLocationId)?.Name,
                    ParentLocationName = _context.Location.FirstOrDefault(p => p.Id == loc.ParentLocationId)?.Name,
                    EventName = _context.Event.FirstOrDefault(e => e.Id == loc.EventId)?.Name,
                    LanguageName = _context.Language.FirstOrDefault(l => l.Id == loc.LanguageId)?.Type,
                    SnapshotName = _context.Snapshot.FirstOrDefault(s => s.Id == loc.SnapshotId)?.SnapshotName
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Location());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns();
            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            using var reader = new StreamReader(Request.Body);
            var rawJson = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(rawJson))
                return BadRequest("No data received");

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(rawJson);
            if (data == null)
                return BadRequest("Invalid JSON");

            var location = await _context.Location.FindAsync(id);
            if (location == null)
                return NotFound();

            location.Name = data.GetValueOrDefault("Name");
            location.Type = data.GetValueOrDefault("Type");
            location.Biome = data.GetValueOrDefault("Biome");
            location.Cultures = data.GetValueOrDefault("Cultures");
            location.Politics = data.GetValueOrDefault("Politics");

            location.TotalPopulation = ParseNullableInt(data.GetValueOrDefault("TotalPopulation"));
            location.DivineMagicians = ParseNullableInt(data.GetValueOrDefault("DivineMagicians"));
            location.WildMagicians = ParseNullableInt(data.GetValueOrDefault("WildMagicians"));
            location.ChildLocationId = ParseNullableInt(data.GetValueOrDefault("ChildLocationId"));
            location.ParentLocationId = ParseNullableInt(data.GetValueOrDefault("ParentLocationId"));
            location.EventId = ParseNullableInt(data.GetValueOrDefault("EventId"));
            location.LanguageId = ParseNullableInt(data.GetValueOrDefault("LanguageId"));
            location.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));

            _context.Update(location);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Location updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Location.FindAsync(id);
            if (location == null) return NotFound();
            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);
            if (location != null)
            {
                _context.Location.Remove(location);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewData["ChildLocationIdList"] = await _context.Location
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToListAsync();

            ViewData["ParentLocationIdList"] = await _context.Location
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToListAsync();

            ViewData["EventIdList"] = await _context.Event
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name })
                .ToListAsync();

            ViewData["LanguageIdList"] = await _context.Language
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Type })
                .ToListAsync();

            ViewData["SnapshotIdList"] = await _context.Snapshot
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SnapshotName })
                .ToListAsync();
        }

        private int? ParseNullableInt(string? value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}
