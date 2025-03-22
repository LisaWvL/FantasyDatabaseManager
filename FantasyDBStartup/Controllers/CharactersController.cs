using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDBStartup.Controllers
{
    public class CharactersController : Controller
    {
        private readonly AppDbContext _context;

        public CharactersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Display List of Characters
        public async Task<IActionResult> Index()
        {
            var characters = await _context.Characters
                .Include(c => c.Snapshot)  // Ensure related data is loaded
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .ToListAsync();

            if (characters == null || characters.Count == 0)
            {
                ViewData["NoDataMessage"] = "No characters found in the database.";
            }

            return View(characters);
        }

        // ✅ GET: Characters/Create (Load Form)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["SnapshotId"] = new SelectList(
                await _context.Snapshots
                    .Select(s => new { s.Id, BookActChapter = s.Book + " - " + s.Act + " - " + s.Chapter })
                    .ToListAsync(),
                "Id", "BookActChapter"
            );

            ViewData["FactionId"] = new SelectList(await _context.Factions.ToListAsync(), "Id", "Name");

            ViewData["LocationId"] = new SelectList(
                await _context.Locations
                    .Select(l => new { l.Id, DisplayName = l.Type + " - " + l.Name })
                    .ToListAsync(),
                "Id", "DisplayName"
            );

            return View();
        }

        // ✅ POST: Characters/Create (Submit Form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Characters.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Redirect to character list
            }

            // Reload dropdowns if validation fails
            ViewData["SnapshotId"] = new SelectList(
                await _context.Snapshots
                    .Select(s => new { s.Id, BookActChapter = s.Book + " - " + s.Act + " - " + s.Chapter })
                    .ToListAsync(),
                "Id", "BookActChapter", character.SnapshotId
            );

            ViewData["FactionId"] = new SelectList(await _context.Factions.ToListAsync(), "Id", "Name", character.FactionId);

            ViewData["LocationId"] = new SelectList(
                await _context.Locations
                    .Select(l => new { l.Id, DisplayName = l.Type + " - " + l.Name })
                    .ToListAsync(),
                "Id", "DisplayName", character.LocationId
            );

            return View(character);
        }

        // ✅ GET: Characters/GetCharacter/{id} (API Endpoint)
        [HttpGet("Characters/GetCharacter/{id}")]
        public async Task<IActionResult> GetCharacter(int id)
        {
            var character = await _context.Characters
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Snapshot)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }
    }
}
