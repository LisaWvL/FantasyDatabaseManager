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
    public class SnapshotController : Controller
    {
        private readonly AppDbContext _context;

        public SnapshotController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var snapshots = await _context.Snapshot.AsNoTracking().ToListAsync();

            var viewModel = snapshots
                .Select(s => new SnapshotViewModel
                {
                    Id = s.Id,
                    Book = s.Book,
                    Act = s.Act,
                    Chapter = s.Chapter,
                    SnapshotName = s.SnapshotName // ✅ Now from DB column
                })
                .ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new Snapshot());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Snapshot snapshot)
        {
            if (ModelState.IsValid)
            {
                _context.Snapshot.Add(snapshot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(snapshot);
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

            var snapshot = await _context.Snapshot.FindAsync(id);
            if (snapshot == null)
                return NotFound();

            snapshot.Book = data.GetValueOrDefault("Book") ?? string.Empty;
            snapshot.Act = data.GetValueOrDefault("Act") ?? string.Empty;
            snapshot.Chapter = data.GetValueOrDefault("Chapter") ?? string.Empty;

            // SnapshotName is computed in DB column definition

            _context.Update(snapshot);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Snapshot updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var snapshot = await _context.Snapshot.FindAsync(id);
            if (snapshot == null) return NotFound();

            return View(snapshot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var snapshot = await _context.Snapshot.FindAsync(id);
            if (snapshot != null)
            {
                _context.Snapshot.Remove(snapshot);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
