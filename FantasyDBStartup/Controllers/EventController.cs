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
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Event
                .AsNoTracking()
                .ToListAsync();

            var viewModel = events
                .AsEnumerable()
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Day = e.Day,
                    Month = e.Month,
                    Year = e.Year,
                    Purpose = e.Purpose,
                    SnapshotId = e.SnapshotId,
                    LocationId = e.LocationId,
                    SnapshotName = _context.Snapshot.FirstOrDefault(s => s.Id == e.SnapshotId)?.SnapshotName,
                    LocationName = _context.Location.FirstOrDefault(l => l.Id == e.LocationId)?.Name
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Event());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (ModelState.IsValid)
            {
                _context.Event.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDropdowns();
            return View(model);
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

            var ev = await _context.Event.FindAsync(id);
            if (ev == null)
                return NotFound();

            ev.Name = data.GetValueOrDefault("Name");
            ev.Description = data.GetValueOrDefault("Description");
            ev.Day = ParseNullableInt(data.GetValueOrDefault("Day"));
            ev.Month = data.GetValueOrDefault("Month");
            ev.Year = ParseNullableInt(data.GetValueOrDefault("Year"));
            ev.Purpose = data.GetValueOrDefault("Purpose");
            ev.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));
            ev.LocationId = ParseNullableInt(data.GetValueOrDefault("LocationId"));

            _context.Update(ev);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Event.FindAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Event.FindAsync(id);
            if (ev != null)
            {
                _context.Event.Remove(ev);
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

            ViewData["LocationIdList"] = await _context.Location
                .AsNoTracking()
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();
        }

        private int? ParseNullableInt(string? value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}
