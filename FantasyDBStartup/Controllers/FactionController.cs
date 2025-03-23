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
    [Route("api/faction")]
    public class FactionController : BaseEntityController<Faction, FactionViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IDropdownService _dropdownService;

        public FactionController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<Faction> GetQueryable() => _context.Faction;

        //Override Index to include Related Names
        public override async Task<IActionResult> Index()
        {
            var factions = await _context.Faction
                .Include(f => f.Founder)
                .Include(f => f.Leader)
                .Include(f => f.HQLocation)
                .Include(f => f.Snapshot)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<FactionViewModel>>(factions);

            ViewData["CurrentEntity"] = "Faction";
            await LoadDropdownsForViewModel<FactionViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] FactionViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var faction = await _context.Faction.FindAsync(id);
            if (faction == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, faction);

            _context.Faction.Update(faction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Faction updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var faction = await _context.Faction.FindAsync(id);
            if (faction == null)
            {
                return NotFound();
            }

            _context.Faction.Remove(faction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Faction deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] FactionViewModel viewModel)
        {
            await LoadDropdownsForViewModel<FactionViewModel>();
            return await base.Create(viewModel);
        }

    }
}