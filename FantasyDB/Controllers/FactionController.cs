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
    [Route("api/faction")]
    public class FactionController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Faction, FactionViewModel>(context, mapper, dropdownService)
    {

        // ✅ EF Core query with full includes
        protected override IQueryable<Faction> GetQueryable()
        {
            return _context.Factions
                .Include(f => f.Founder)
                .Include(f => f.Leader)
                .Include(f => f.HQLocation)
                .Include(f => f.Chapter);
        }

        // ✅ API-compatible Index method
        public override async Task<ActionResult<List<FactionViewModel>>> Index()
        {
            var factions = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<FactionViewModel>>(factions);
            return Ok(viewModels);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<FactionViewModel>> Create([FromBody] FactionViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] FactionViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
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
