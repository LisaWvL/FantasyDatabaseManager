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
    public class RiverController : Controller
    {
        private readonly AppDbContext _context;

        public RiverController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rivers = await _context.River.AsNoTracking().ToListAsync();

            var viewModel = rivers
                .Select(r => new RiverViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    DepthMeters = r.DepthMeters,
                    WidthMeters = r.WidthMeters,
                    FlowDirection = r.FlowDirection,
                    SourceLocationId = r.SourceLocationId,
                    DestinationLocationId = r.DestinationLocationId,
                    SourceLocationName = _context.Location.FirstOrDefault(l => l.Id == r.SourceLocationId)?.Name,
                    DestinationLocationName = _context.Location.FirstOrDefault(l => l.Id == r.DestinationLocationId)?.Name
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new River());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(River model)
        {
            if (ModelState.IsValid)
            {
                _context.River.Add(model);
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

            var river = await _context.River.FindAsync(id);
            if (river == null)
                return NotFound();

            river.Name = data.GetValueOrDefault("Name");
            river.FlowDirection = data.GetValueOrDefault("FlowDirection");
            river.DepthMeters = ParseNullableInt(data.GetValueOrDefault("DepthMeters"));
            river.WidthMeters = ParseNullableInt(data.GetValueOrDefault("WidthMeters"));
            river.SourceLocationId = ParseNullableInt(data.GetValueOrDefault("SourceLocationId"));
            river.DestinationLocationId = ParseNullableInt(data.GetValueOrDefault("DestinationLocationId"));

            _context.Update(river);
            await _context.SaveChangesAsync();

            return Ok(new { message = "River updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.River.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.River.FindAsync(id);
            if (item != null)
            {
                _context.River.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            var locations = await _context.Location
                .AsNoTracking()
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();

            ViewData["SourceLocationIdList"] = locations;
            ViewData["DestinationLocationIdList"] = locations;
        }

        private int? ParseNullableInt(string? value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}
