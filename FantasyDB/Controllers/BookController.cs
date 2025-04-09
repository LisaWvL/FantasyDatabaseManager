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
    }
}