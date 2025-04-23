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
    [Route("api/Item")]
    public class ItemController : BaseEntityController<Item, ItemViewModel>
    {
        public ItemController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }
        protected override IQueryable<Item> GetQueryable()
        {
            return _context.Items
                .Include(a => a.Owner)
                .Include(a => a.Chapter);
        }

        // ✅ Electron-compatible: Full list of Items with includes
        public override async Task<ActionResult<List<ItemViewModel>>> Index()
        {
            var Items = await _context.Items
                .Include(a => a.Owner)
                .Include(a => a.Chapter)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<ItemViewModel>>(Items);
            return Ok(viewModels);
        }

        // ✅ Electron-compatible: Create Item and return result
        [HttpPost("create")]
        public override async Task<ActionResult<ItemViewModel>> Create([FromBody] ItemViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        // ✅ Electron-compatible: Update Item
        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] ItemViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        // ✅ Electron-compatible: Delete Item
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        // ✅ Optional: Use for chapter duplication logic
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

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Items.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

    }
}
