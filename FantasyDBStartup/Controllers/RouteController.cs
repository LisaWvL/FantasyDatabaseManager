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
    public class RouteController : Controller
    {
        private readonly AppDbContext _context;

        public RouteController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var routes = await _context.Route.AsNoTracking().ToListAsync();

            var viewModel = routes
                .Select(r => new RouteViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Type = r.Type,
                    Length = r.Length,
                    Notes = r.Notes,
                    TravelTime = r.TravelTime,
                    FromId = r.FromId,
                    ToId = r.ToId,
                    FromLocationName = _context.Location.FirstOrDefault(l => l.Id == r.FromId)?.Name,
                    ToLocationName = _context.Location.FirstOrDefault(l => l.Id == r.ToId)?.Name
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new FantasyDB.Models.Route());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FantasyDB.Models.Route model)
        {
            if (ModelState.IsValid)
            {
                _context.Route.Add(model);
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

            var route = await _context.Route.FindAsync(id);
            if (route == null)
                return NotFound();

            route.Name = data.GetValueOrDefault("Name");
            route.Type = data.GetValueOrDefault("Type");
            route.Notes = data.GetValueOrDefault("Notes");
            route.TravelTime = data.GetValueOrDefault("TravelTime");
            route.Length = ParseNullableInt(data.GetValueOrDefault("Length"));
            route.FromId = ParseNullableInt(data.GetValueOrDefault("FromId"));
            route.ToId = ParseNullableInt(data.GetValueOrDefault("ToId"));

            _context.Update(route);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Route updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var route = await _context.Route.FindAsync(id);
            if (route == null) return NotFound();
            return View(route);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Route.FindAsync(id);
            if (route != null)
            {
                _context.Route.Remove(route);
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

            ViewData["FromIdList"] = locations;
            ViewData["ToIdList"] = locations;
        }

        private int? ParseNullableInt(string? value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}
