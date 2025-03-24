using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDBStartup.Controllers
{
    [Route("api/language")]
    public class LanguageController : BaseEntityController<Language, LanguageViewModel>
    {

        public LanguageController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }


        protected override IQueryable<Language> GetQueryable()
        { return _context.Language; }

        //Override Index to include Location Name
        public override async Task<IActionResult> Index()
        {
            var languages = await _context.Language
                .Include(l => l.LanguageLocations)
                    .ThenInclude(ll => ll.Location)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<LanguageViewModel>>(languages);

            ViewData["CurrentEntity"] = "Language";
            await LoadDropdownsForViewModel<LanguageViewModel>();

            return View("_EntityTable", viewModels);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] LanguageViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
                return BadRequest("Invalid request");

            var language = await _context.Language
                .Include(l => l.LanguageLocations)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (language == null)
                return NotFound();

            _mapper.Map(viewModel, language);

            // ✅ Handle junction table
            language.LanguageLocations.Clear();
            if (viewModel.LocationIds != null)
            {
                language.LanguageLocations = viewModel.LocationIds
                    .Select(locId => new LanguageLocation
                    {
                        LanguageId = language.Id,
                        LocationId = locId
                    })
                    .ToList();
            }

            _context.Language.Update(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language updated successfully" });
        }

        public override async Task<IActionResult> Create([FromBody] LanguageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Invalid model:\n" + errors);
            }

            var language = _mapper.Map<Language>(viewModel);

            if (viewModel.LocationIds != null)
            {
                language.LanguageLocations = viewModel.LocationIds
                    .Select(locId => new LanguageLocation
                    {
                        LocationId = locId
                    }).ToList();
            }

            _context.Language.Add(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language created" });
        }


        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var language = await _context.Language.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            _context.Language.Remove(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language deleted" });
        }


        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] LanguageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Invalid model:\n" + errors);
            }

            var language = await _context.Language
                .Include(l => l.LanguageLocations)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (language == null) return NotFound();

            _mapper.Map(viewModel, language);

            // Manually handle junction entries
            language.LanguageLocations.Clear();
            if (viewModel.LocationIds != null)
            {
                foreach (var locId in viewModel.LocationIds)
                {
                    language.LanguageLocations.Add(new LanguageLocation
                    {
                        LanguageId = id,
                        LocationId = locId
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Language updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest("Save failed: " + ex.Message);
            }
        }

        [HttpGet("{id}/new-snapshot")]
        public override async Task<IActionResult> CreateNewSnapshot(int id)
        {
            return await base.CreateNewSnapshot(id);
        }

        [HttpGet("{id}/new-snapshot-page")]
        public override async Task<IActionResult> CreateNewSnapshotPage(int id)
        {
            return await base.CreateNewSnapshotPage(id);
        }

    }
}