using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Language, LanguageViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Language> GetQueryable()
        {
            return _context.Languages
                .Include(l => l.LanguageLocations) // this is enough!
                    .ThenInclude(ll => ll.Location);   // optionally include the related Location data
        }


        public override async Task<ActionResult<List<LanguageViewModel>>> Index()
        {
            var languages = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<LanguageViewModel>>(languages);
            return Ok(viewModels);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<LanguageViewModel>> Create([FromBody] LanguageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Invalid model:\n" + errors);
            }

            var language = _mapper.Map<Language>(viewModel);

            // Handle junction
            if (viewModel.LocationIds != null)
            {
                language.LanguageLocations = [.. viewModel.LocationIds.Select(locId => new LanguageLocation { LocationId = locId })];
            }

            _context.Languages.Add(language);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<LanguageViewModel>(language));
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] LanguageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Invalid model:\n" + errors);
            }

            var language = await _context.Languages
                .Include(l => l.LanguageLocations)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (language == null) return NotFound();

            _mapper.Map(viewModel, language);

            // Refresh junctions
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

            await _context.SaveChangesAsync();
            return Ok(new { message = "Language updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var language = await _context.Languages.FindAsync(id);
            if (language == null)
                return NotFound();

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language deleted" });
        }

        [HttpGet("{id}/new-chapter")]
        public override async Task<IActionResult> CreateNewChapter(int id)
        {
            return await base.CreateNewChapter(id);
        }

        [HttpGet("{id}/new-chapter-page")]
        public override async Task<IActionResult> CreateNewWritingAssistantPage(int id)
        {
            return await base.CreateNewWritingAssistantPage(id);
        }
    }
}
