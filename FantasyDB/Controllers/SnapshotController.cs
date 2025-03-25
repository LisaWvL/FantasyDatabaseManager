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
    [Route("api/snapshot")]
    public class SnapshotController : BaseEntityController<Snapshot, SnapshotViewModel>
    {
        public SnapshotController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        protected override IQueryable<Snapshot> GetQueryable()
        {
            return _context.Snapshots;
        }

        public override async Task<ActionResult<List<SnapshotViewModel>>> Index()
        {
            var snapshots = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<SnapshotViewModel>>(snapshots);
            return Ok(viewModels);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<SnapshotViewModel>> Create([FromBody] SnapshotViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] SnapshotViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
