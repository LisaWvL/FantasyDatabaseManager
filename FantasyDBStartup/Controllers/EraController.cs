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
    public class EraController : Controller
    {
        private readonly AppDbContext _context;

        public EraController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var eras = await _context.Era
                .AsNoTracking()
                .ToListAsync();

            var viewModel = eras
                .AsEnumerable()
                .Select(e => new EraViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear,
                    MagicSystem = e.MagicSystem,
                    MagicStatus = e.MagicStatus,
                    SnapshotId = e.SnapshotId,
                    SnapshotName = _context.Snapshot.FirstOrDefault(s => s.Id == e.SnapshotId)?.SnapshotName
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Era());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Era era)
        {
            if (ModelState.IsValid)
            {
                _context.Era.Add(era);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDropdowns();
            return View(era);
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

            var era = await _context.Era.FindAsync(id);
            if (era == null)
                return NotFound();

            era.Name = data.GetValueOrDefault("Name");
            era.Description = data.GetValueOrDefault("Description");
            era.StartYear = ParseNullableInt(data.GetValueOrDefault("StartYear"));
            era.EndYear = ParseNullableInt(data.GetValueOrDefault("EndYear"));
            era.MagicSystem = data.GetValueOrDefault("MagicSystem");
            era.MagicStatus = data.GetValueOrDefault("MagicStatus");
            era.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));

            _context.Era.Update(era);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Era updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var era = await _context.Era.FindAsync(id);
            if (era == null) return NotFound();
            return View(era);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var era = await _context.Era.FindAsync(id);
            if (era != null)
            {
                _context.Era.Remove(era);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewData["SnapshotIdList"] = await _context.Snapshot
                .AsNoTracking()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SnapshotName
                })
                .ToListAsync();
        }

        private int? ParseNullableInt(string? value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}
