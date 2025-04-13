using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Chapter, ChapterViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Chapter> GetQueryable()
        {
            return _context.Chapters;
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