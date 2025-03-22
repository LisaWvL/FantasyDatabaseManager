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
    public class CharacterController : Controller
    {
        private readonly AppDbContext _context;

        public CharacterController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var characters = await _context.Character
                .AsNoTracking()
                .ToListAsync(); // Fetch from DB first to avoid translation errors

            var viewModel = characters
                .AsEnumerable() // ✅ Forces client-side evaluation
                .Select(c => new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Role = c.Role,
                    Alias = c.Alias,
                    BirthDay = c.BirthDay,
                    BirthMonth = c.BirthMonth,
                    BirthYear = c.BirthYear,
                    Gender = c.Gender,
                    HeightCm = c.HeightCm,
                    Build = c.Build,
                    Hair = c.Hair,
                    Eyes = c.Eyes,
                    DefiningFeatures = c.DefiningFeatures,
                    Personality = c.Personality,
                    SocialStatus = c.SocialStatus,
                    Occupation = c.Occupation,
                    Magic = c.Magic,
                    Desire = c.Desire,
                    Fear = c.Fear,
                    Weakness = c.Weakness,
                    Motivation = c.Motivation,
                    Flaw = c.Flaw,
                    Misbelief = c.Misbelief,
                    SnapshotId = c.SnapshotId, 
                    FactionId = c.FactionId, 
                    LocationId = c.LocationId, 
                    LanguageId = c.LanguageId,

                    SnapshotName = _context.Snapshot.Where(s => s.Id == c.SnapshotId).Select(s => s.SnapshotName).FirstOrDefault(),

                    FactionName = _context.Faction.Where(f => f.Id == c.FactionId).Select(f => f.Name).FirstOrDefault(),

                    LocationName = _context.Location.Where(l => l.Id == c.LocationId).Select(l => l.Name).FirstOrDefault(),

                    LanguageName = _context.Language.Where(lang => lang.Id == c.LanguageId).Select(lang => lang.Type).FirstOrDefault()
                })
                .ToList();

            await LoadDropdowns(); // ? Ensures ViewBag.SnapshotList is set

            ViewData["Snapshots"] = await _context.Snapshot.ToListAsync();
            ViewData["Factions"] = await _context.Faction.ToListAsync();
            ViewData["Locations"] = await _context.Location.ToListAsync();
            ViewData["Languages"] = await _context.Language.ToListAsync();

            return View(viewModel);
        }


        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Character());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDropdowns();
            return View(character);
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var character = await _context.Character.FindAsync(id);
        //    await LoadDropdowns();
        //    return View(character);
        //}


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

            var character = await _context.Character.FindAsync(id);
            if (character == null)
                return NotFound();

            // Apply updates (carefully parse ints)
            character.Name = data.GetValueOrDefault("Name");
            character.Role = data.GetValueOrDefault("Role");
            character.Alias = data.GetValueOrDefault("Alias");
            character.Gender = data.GetValueOrDefault("Gender");
            character.Build = data.GetValueOrDefault("Build");
            character.Hair = data.GetValueOrDefault("Hair");
            character.Eyes = data.GetValueOrDefault("Eyes");
            character.DefiningFeatures = data.GetValueOrDefault("DefiningFeatures");
            character.Personality = data.GetValueOrDefault("Personality");
            character.SocialStatus = data.GetValueOrDefault("SocialStatus");
            character.Occupation = data.GetValueOrDefault("Occupation");
            character.Magic = data.GetValueOrDefault("Magic");
            character.Desire = data.GetValueOrDefault("Desire");
            character.Fear = data.GetValueOrDefault("Fear");
            character.Weakness = data.GetValueOrDefault("Weakness");
            character.Motivation = data.GetValueOrDefault("Motivation");
            character.Flaw = data.GetValueOrDefault("Flaw");
            character.Misbelief = data.GetValueOrDefault("Misbelief");

            // Optional parsing for nullable ints
            character.HeightCm = ParseNullableInt(data.GetValueOrDefault("HeightCm"));
            character.BirthDay = ParseNullableInt(data.GetValueOrDefault("BirthDay"));
            character.BirthYear = ParseNullableInt(data.GetValueOrDefault("BirthYear"));

            character.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));
            character.FactionId = ParseNullableInt(data.GetValueOrDefault("FactionId"));
            character.LocationId = ParseNullableInt(data.GetValueOrDefault("LocationId"));
            character.LanguageId = ParseNullableInt(data.GetValueOrDefault("LanguageId"));

            _context.Update(character);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Character updated directly" });
        }

        private int? ParseNullableInt(string? value)
        {
            if (int.TryParse(value, out var result))
                return result;
            return null;
        }




        private async Task LoadDropdowns()
        {
            ViewData["SnapshotIdList"] = await _context.Snapshot
                .AsNoTracking()
                .OrderBy(s => s.SnapshotName)
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SnapshotName })
                .ToListAsync();


            // ✅ Fetch other dropdowns normally
            ViewData["FactionIdList"] = await _context.Faction
        .AsNoTracking()
        .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
        .ToListAsync();

            ViewData["LocationIdList"] = await _context.Location
                .AsNoTracking()
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToListAsync();

            ViewData["LanguageIdList"] = await _context.Language
                .AsNoTracking()
                .Select(lang => new SelectListItem { Value = lang.Id.ToString(), Text = lang.Type })
                .ToListAsync();
        }
    }
}
