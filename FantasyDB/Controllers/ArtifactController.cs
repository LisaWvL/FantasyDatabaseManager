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
    [Route("api/artifact")]
    public class ArtifactController : BaseEntityController<Artifact, ArtifactViewModel>
    {
        public ArtifactController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }
        protected override IQueryable<Artifact> GetQueryable()
        {
            return _context.Artifacts
                .Include(a => a.Owner)
                .Include(a => a.Snapshot);
        }

        // ✅ Electron-compatible: Full list of artifacts with includes
        public override async Task<ActionResult<List<ArtifactViewModel>>> Index()
        {
            var artifacts = await _context.Artifacts
                .Include(a => a.Owner)
                .Include(a => a.Snapshot)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<ArtifactViewModel>>(artifacts);
            return Ok(viewModels);
        }

        // ✅ Electron-compatible: Create artifact and return result
        [HttpPost("create")]
        public override async Task<ActionResult<ArtifactViewModel>> Create([FromBody] ArtifactViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        // ✅ Electron-compatible: Update artifact
        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] ArtifactViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        // ✅ Electron-compatible: Delete artifact
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
