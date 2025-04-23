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
        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Rivers.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
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
