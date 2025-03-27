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
                .Include(a => a.Snapshot);
        }

        // ✅ Electron-compatible: Full list of Items with includes
        public override async Task<ActionResult<List<ItemViewModel>>> Index()
        {
            var Items = await _context.Items
                .Include(a => a.Owner)
                .Include(a => a.Snapshot)
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

        // ✅ Optional: Use for snapshot duplication logic
        [HttpGet("{id}/new-snapshot")]
        public override async Task<IActionResult> CreateNewSnapshot(int id)
        {
            return await base.CreateNewSnapshot(id);
        }

        [HttpGet("{id}/new-snapshot-page")]
        public override async Task<IActionResult> CreateNewSnapshotPage(int id)
        {
            return await base.CreateNewSnapshotPage(id);
        }
    }
}
