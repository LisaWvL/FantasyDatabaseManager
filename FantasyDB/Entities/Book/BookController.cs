using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Entities._Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Entities
{
    [ApiController]
    [Route("api/book")]
    public class BookController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
    : BaseEntityController<Book, BookViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Book> GetQueryable()
        {
            return _context.Books
                .Include(b => b.Acts)
                .ThenInclude(a => a.Chapters);
        }

        [HttpGet("{id}/wordcount")]
        public async Task<ActionResult<int>> GetBookWordCount(int id)
        {
            var count = await _context.Chapters
                .Where(c => c.Act!.BookId == id)
                .SumAsync(c => c.WordCount ?? 0);
            return Ok(count);
        }

        [HttpPost("{bookId}/reorder-acts")]
        public async Task<IActionResult> ReorderActs(int bookId, [FromBody] List<int> orderedActIds)
        {
            var acts = await _context.Acts
                .Where(a => a.BookId == bookId)
                .ToListAsync();

            for (int i = 0; i < orderedActIds.Count; i++)
            {
                var act = acts.FirstOrDefault(a => a.Id == orderedActIds[i]);
                if (act != null)
                    act.ActNumber = i + 1;
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Books.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }


    }
}