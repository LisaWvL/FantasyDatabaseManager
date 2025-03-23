using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace FantasyDBStartup.Controllers
{
    [Route("api/artifact")]
    public class ArtifactController : BaseEntityController<Artifact, ArtifactViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IDropdownService _dropdownService;

        public ArtifactController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }


        protected override IQueryable<Artifact> GetQueryable() => _context.Artifact;

        //Override Index to include owner and snapshot name
        public override async Task<IActionResult> Index()
        {
            var artifacts = await _context.Artifact
                .Include(a => a.Owner)
                .Include(a => a.Snapshot)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<ArtifactViewModel>>(artifacts);

            ViewData["CurrentEntity"] = "Artifact";
            await LoadDropdownsForViewModel<ArtifactViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] ArtifactViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var artifact = await _context.Artifact.FindAsync(id);
            if (artifact == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, artifact);

            _context.Artifact.Update(artifact);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Artifact updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var artifact = await _context.Artifact.FindAsync(id);
            if (artifact == null)
            {
                return NotFound();
            }

            _context.Artifact.Remove(artifact);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Artifact deleted" });
        }
        public async override Task<IActionResult> Create([FromBody] ArtifactViewModel viewModel)
        {
            await LoadDropdownsForViewModel<ArtifactViewModel>();

            return await base.Create(viewModel);
        }
    }
}