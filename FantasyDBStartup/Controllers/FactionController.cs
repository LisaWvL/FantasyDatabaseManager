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
    public class FactionController : Controller
    {
        private readonly AppDbContext _context;

        public FactionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var factions = await _context.Faction
                .AsNoTracking()
                .ToListAsync();

            var viewModel = factions
                .AsEnumerable()
                .Select(f => new FactionViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Alias = f.Alias,
                    Magic = f.Magic,
                    FoundingYear = f.FoundingYear,

                    FounderId = f.FounderId,
                    LeaderId = f.LeaderId,
                    HQLocationId = f.HQLocationId,
                    SnapshotId = f.SnapshotId,

                    FounderName = _context.Character
                        .Where(c => c.Id == f.FounderId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),

                    LeaderName = _context.Character
                        .Where(c => c.Id == f.LeaderId)
                        .Select(c => c.Name)
                        .FirstOrDefault(),

                    HQLocationName = _context.Location
                        .Where(l => l.Id == f.HQLocationId)
                        .Select(l => l.Name)
                        .FirstOrDefault(),

                    SnapshotName = _context.Snapshot
                        .Where(s => s.Id == f.SnapshotId)
                        .Select(s => s.SnapshotName)
                        .FirstOrDefault()
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Faction());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faction faction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDropdowns();
            return View(faction);
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

            var faction = await _context.Faction.FindAsync(id);
            if (faction == null)
                return NotFound();

            faction.Name = data.GetValueOrDefault("Name");
            faction.Alias = data.GetValueOrDefault("Alias");
            faction.Magic = data.GetValueOrDefault("Magic");
            faction.FoundingYear = ParseNullableInt(data.GetValueOrDefault("FoundingYear"));

            faction.FounderId = ParseNullableInt(data.GetValueOrDefault("FounderId"));
            faction.LeaderId = ParseNullableInt(data.GetValueOrDefault("LeaderId"));
            faction.HQLocationId = ParseNullableInt(data.GetValueOrDefault("HQLocationId"));
            faction.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));

            _context.Update(faction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Faction updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var faction = await _context.Faction.FindAsync(id);
            if (faction == null) return NotFound();
            return View(faction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faction = await _context.Faction.FindAsync(id);
            if (faction != null)
            {
                _context.Faction.Remove(faction);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewData["FounderIdList"] = await _context.Character
                .AsNoTracking()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            ViewData["LeaderIdList"] = await _context.Character
                .AsNoTracking()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            ViewData["HQLocationIdList"] = await _context.Location
                .AsNoTracking()
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToListAsync();

            ViewData["SnapshotIdList"] = await _context.Snapshot
                .AsNoTracking()
                .OrderBy(s => s.SnapshotName)
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SnapshotName })
                .ToListAsync();
        }

        private int? ParseNullableInt(string? value)
        {
            if (int.TryParse(value, out var result))
                return result;
            return null;
        }
    }
}
