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
    public class SnapshotController : BaseEntityController<Snapshot, SnapshotViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDropdownService _dropdownService;

        public SnapshotController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<Snapshot> GetQueryable() => _context.Snapshot;

        //Override index to use Automapper
        public override async Task<IActionResult> Index()
        {
            var snapshots = await _context.Snapshot
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<SnapshotViewModel>>(snapshots);

            ViewData["CurrentEntity"] = "Snapshot";
            await LoadDropdownsForViewModel<SnapshotViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] SnapshotViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var snapshot = await _context.Snapshot.FindAsync(id);
            if (snapshot == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, snapshot);

            _context.Snapshot.Update(snapshot);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Snapshot updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var snapshot = await _context.Snapshot.FindAsync(id);
            if (snapshot == null)
            {
                return NotFound();
            }

            _context.Snapshot.Remove(snapshot);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Snapshot deleted" });
        }
    }
}