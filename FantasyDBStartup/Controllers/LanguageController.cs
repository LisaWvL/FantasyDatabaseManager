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
    public class LanguageController : Controller
    {
        private readonly AppDbContext _context;

        public LanguageController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var languages = await _context.Language
                .AsNoTracking()
                .ToListAsync();

            var viewModel = languages
                .AsEnumerable()
                .Select(l => new LanguageViewModel
                {
                    Id = l.Id,
                    Type = l.Type,
                    Text = l.Text,
                    LocationId = l.LocationId,
                    LanguageName = l.LanguageName,
                    Location = _context.Location.FirstOrDefault(loc => loc.Id == l.LocationId)
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Language());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Language model)
        {
            if (ModelState.IsValid)
            {
                _context.Language.Add(model);
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

            var language = await _context.Language.FindAsync(id);
            if (language == null)
                return NotFound();

            language.Type = data.GetValueOrDefault("Type");
            language.Text = data.GetValueOrDefault("Text");
            language.LanguageName = data.GetValueOrDefault("LanguageName");
            language.LocationId = ParseNullableInt(data.GetValueOrDefault("LocationId"));

            _context.Update(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var language = await _context.Language.FindAsync(id);
            if (language == null) return NotFound();
            return View(language);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var language = await _context.Language.FindAsync(id);
            if (language != null)
            {
                _context.Language.Remove(language);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
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
