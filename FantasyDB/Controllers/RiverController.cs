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
    [Route("api/river")]
    public class RiverController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<River, RiverViewModel>(context, mapper, dropdownService)
    {

        // ✅ Include source and destination locations
        protected override IQueryable<River> GetQueryable()
        {
            return _context.Rivers
                .Include(r => r.SourceLocation)
                .Include(r => r.DestinationLocation);
        }

        // ✅ Electron-compatible return type
        public override async Task<ActionResult<List<RiverViewModel>>> Index()
        {
            var rivers = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<RiverViewModel>>(rivers);
            return Ok(viewModels);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<RiverViewModel>> Create([FromBody] RiverViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] RiverViewModel viewModel)
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
