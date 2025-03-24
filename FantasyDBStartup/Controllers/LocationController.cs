using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDBStartup.Controllers
{
    [Route("api/location")]
    public class LocationController : BaseEntityController<Location, LocationViewModel>
    {

        public LocationController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        protected override IQueryable<Location> GetQueryable()
        { return _context.Location; }

        //Override Index to include Related Names
        public override async Task<IActionResult> Index()
        {
            var locations = await _context.Location
                .Include(l => l.ParentLocation)
                .Include(l => l.LocationLocations)
                    .ThenInclude(ll => ll.Location)
                .Include(l => l.LocationEvents)
                    .ThenInclude(ll => ll.Location)
                .Include(l => l.Language)
                .Include(l => l.Snapshot)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<LocationViewModel>>(locations);

            ViewData["CurrentEntity"] = "Location";
            await LoadDropdownsForViewModel<LocationViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] LocationViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, location);

            _context.Location.Update(location);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Location updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Location.Remove(location);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Location deleted" });
        }

        [HttpPost("create")]
        public override async Task<IActionResult> Create([FromBody] LocationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _mapper.Map<Location>(viewModel);
            _context.Location.Add(location);
            await _context.SaveChangesAsync(); // Save first to get Location.Id

            // --- Add Location-ChildLocation relationships ---
            if (viewModel.ChildLocationIds != null)
            {
                foreach (var childId in viewModel.ChildLocationIds)
                {
                    _context.LocationLocation.Add(new LocationLocation
                    {
                        LocationId = location.Id,
                        ChildLocationId = childId
                    });
                }
            }

            // --- Add Location-Event relationships ---
            if (viewModel.EventIds != null)
            {
                foreach (var eventId in viewModel.EventIds)
                {
                    _context.LocationEvent.Add(new LocationEvent
                    {
                        LocationId = location.Id,
                        EventId = eventId
                    });
                }
            }

            await _context.SaveChangesAsync(); // Save junction tables
            return Ok(new { message = "Location created", id = location.Id });
        }


        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] LocationViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
                return BadRequest();

            var location = await _context.Location.FirstOrDefaultAsync(l => l.Id == id);
            if (location == null)
                return NotFound();

            // Update base properties
            _mapper.Map(viewModel, location);

            // Remove old junctions
            var existingEventLinks = _context.LocationEvent.Where(e => e.LocationId == id);
            var existingChildLinks = _context.LocationLocation.Where(l => l.LocationId == id);
            _context.LocationEvent.RemoveRange(existingEventLinks);
            _context.LocationLocation.RemoveRange(existingChildLinks);

            // Add new Event junctions
            if (viewModel.EventIds != null)
            {
                foreach (var eventId in viewModel.EventIds)
                {
                    _context.LocationEvent.Add(new LocationEvent
                    {
                        LocationId = id,
                        EventId = eventId
                    });
                }
            }

            // Add new ChildLocation junctions
            if (viewModel.ChildLocationIds != null)
            {
                foreach (var childId in viewModel.ChildLocationIds)
                {
                    _context.LocationLocation.Add(new LocationLocation
                    {
                        LocationId = id,
                        ChildLocationId = childId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Location updated" });
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