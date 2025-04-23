using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Entities
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Chapter, ChapterViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Chapter> GetQueryable()
        {
            return _context.Chapters
                 .Include(c => c.StartDate)
                 .Include(c => c.EndDate)
                 .Include(c => c.Act)
                 .Include(c => c.POVCharacter)
                 .Include(c => c.Scenes)
                 .Include(c => c.ChapterPlotPoints)
                 .ThenInclude(cpp => cpp.PlotPoint);

        }

        public override async Task<ActionResult<List<ChapterViewModel>>> Index()
        {
            try
            {
                var chapters = await GetQueryable().AsNoTracking().ToListAsync();
                Console.WriteLine($"[ChapterController] Fetched {chapters.Count} chapter(s)");

                var viewModels = _mapper.Map<List<ChapterViewModel>>(chapters);
                Console.WriteLine($"[ChapterController] Mapped {viewModels.Count} chapter view models");

                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ChapterController.Index failed: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"🔎 Inner: {ex.InnerException.Message}");
                return StatusCode(500, "Chapter fetch failed.");
            }
        }

        [HttpPost("create")]
        public override async Task<ActionResult<ChapterViewModel>> Create([FromBody] ChapterViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] ChapterViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }


        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Chapters.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }


        // ✅ NEW ENDPOINT: All connected entities for a Chapter
        [HttpGet("{id}/entities")]
        public async Task<IActionResult> GetChapterEntities(int id)
        {
            var characters = await _context.Characters
                .Where(c => c.ChapterId == id)
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Language)
                .ToListAsync();

            var factions = await _context.Factions
                .Where(f => f.ChapterId == id)
                .ToListAsync();


            var locations = await _context.Locations
                .Where(f => f.ChapterId == id)
                .ToListAsync();


            var items = await _context.Items
                .Where(f => f.ChapterId == id)
                .Include(f => f.Owner)
                .ToListAsync();


            var relationships = await _context.CharacterRelationships
                .Where(scr => scr.ChapterId == id)
                .Include(scr => scr.Character1)
                .Include(scr => scr.Character2)
                .ToListAsync();

            return Ok(new
            {
                Characters = characters,
                Relationships = relationships,
                Locations = locations,
                Items = items,
                Factions = factions
            });
        }

    }
}