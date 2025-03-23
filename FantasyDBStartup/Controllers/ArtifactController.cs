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
    public class ArtifactController : Controller
    {
        private readonly AppDbContext _context;

        public ArtifactController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artifacts = await _context.Artifact
                .AsNoTracking()
                .ToListAsync();

            var viewModel = artifacts
                .AsEnumerable()
                .Select(a => new ArtifactViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Origin = a.Origin,
                    Effects = a.Effects,
                    OwnerId = a.OwnerId,
                    SnapshotId = a.SnapshotId,
                    OwnerName = _context.Character.Where(c => c.Id == a.OwnerId).Select(c => c.Name).FirstOrDefault(),
                    SnapshotName = _context.Snapshot.Where(s => s.Id == a.SnapshotId).Select(s => s.SnapshotName).FirstOrDefault()
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Artifact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Artifact artifact)
        {
            if (ModelState.IsValid)
            {
                _context.Artifact.Add(artifact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDropdowns();
            return View(artifact);
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

            var artifact = await _context.Artifact.FindAsync(id);
            if (artifact == null)
                return NotFound();

            artifact.Name = data.GetValueOrDefault("Name");
            artifact.Origin = data.GetValueOrDefault("Origin");
            artifact.Effects = data.GetValueOrDefault("Effects");
            artifact.OwnerId = ParseNullableInt(data.GetValueOrDefault("OwnerId"));
            artifact.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));

            _context.Artifact.Update(artifact);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Artifact updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var artifact = await _context.Artifact.FindAsync(id);
            if (artifact == null) return NotFound();

            return View(artifact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artifact = await _context.Artifact.FindAsync(id);
            if (artifact != null)
            {
                _context.Artifact.Remove(artifact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            ViewData["OwnerIdList"] = await _context.Character
                .AsNoTracking()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

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
