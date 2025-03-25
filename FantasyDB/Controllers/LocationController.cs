using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : BaseEntityController<Location, LocationViewModel>
    {
        public LocationController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService) { }

        protected override IQueryable<Location> GetQueryable()
        {
            return _context.Locations
                .Include(l => l.ParentLocation)
                .Include(l => l.Snapshot)
                .Include(l => l.LanguageLocations)
                .Include(l => l.Events)
                    .ThenInclude(le => le.Id);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<LocationViewModel>> Create([FromBody] LocationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _mapper.Map<Location>(viewModel);

            // ✅ Manually set LanguageLocation entries
            location.LanguageLocations = viewModel.LanguageIds
                .Select(langId => new LanguageLocation
                {
                    LanguageId = langId
                    // EF will fill in LocationId after SaveChanges
                }).ToList();

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            // ✅ Reassign existing Events (optional, only if you want to assign Events from EventIds list)
            if (viewModel.EventIds is { Count: > 0 })
            {
                var events = await _context.Events
                    .Where(e => viewModel.EventIds.Contains(e.Id))
                    .ToListAsync();

                foreach (var ev in events)
                {
                    ev.LocationId = location.Id;
                }

                await _context.SaveChangesAsync();
            }

            // ✅ Optional: map the final state back to viewmodel and return
            var resultViewModel = _mapper.Map<LocationViewModel>(location);
            return Ok(resultViewModel);
        }



        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] LocationViewModel viewModel)
        {
            if (!ModelState.IsValid || id != viewModel.Id) return BadRequest();

            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();

            _mapper.Map(viewModel, location);

            // 🔁 Clear existing LanguageLocations:
            location.LanguageLocations.Clear();

            // ✅ Re-add updated ones:
            foreach (var langId in viewModel.LanguageIds)
            {
                location.LanguageLocations.Add(new LanguageLocation
                {
                    LanguageId = langId,
                    LocationId = location.Id
                });
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Location updated", id = location.Id });
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
