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
    [Route("api/act")]
    public class ActController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
     : BaseEntityController<Act, ActViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Act> GetQueryable()
        {
            return _context.Acts
                .Include(a => a.Chapters)
                .ThenInclude(c => c.Scenes);
        }

        [HttpGet("{id}/wordcount")]
        public async Task<ActionResult<int>> GetActWordCount(int id)
        {
            var count = await _context.Chapters
                .Where(c => c.ActId == id)
                .SumAsync(c => c.WordCount ?? 0);
            return Ok(count);
        }

        [HttpPost("{actId}/reorder-chapters")]
        public async Task<IActionResult> ReorderChapters(int actId, [FromBody] List<int> orderedChapterIds)
        {
            var chapters = await _context.Chapters
                .Where(c => c.ActId == actId)
                .ToListAsync();

            for (int i = 0; i < orderedChapterIds.Count; i++)
            {
                var chapter = chapters.FirstOrDefault(c => c.Id == orderedChapterIds[i]);
                if (chapter != null)
                    chapter.ChapterNumber = i + 1;
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }
}