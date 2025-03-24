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
    [Route("api/era")]
    public class EraController : BaseEntityController<Era, EraViewModel>
    {

        public EraController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }


        protected override IQueryable<Era> GetQueryable()
        { return _context.Era; }

        //Override Index to include Related Names
        public override async Task<IActionResult> Index()
        {
            var eras = await _context.Era
                .Include(e => e.Snapshot)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<EraViewModel>>(eras);

            ViewData["CurrentEntity"] = "Era";
            await LoadDropdownsForViewModel<EraViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] EraViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var era = await _context.Era.FindAsync(id);
            if (era == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, era);

            _context.Era.Update(era);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Era updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var era = await _context.Era.FindAsync(id);
            if (era == null)
            {
                return NotFound();
            }

            _context.Era.Remove(era);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Era deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] EraViewModel viewModel)
        {
            await LoadDropdownsForViewModel<EraViewModel>();

            return await base.Create(viewModel);
        }


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