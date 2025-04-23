using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
{
    [Route("api/location")]
    [ApiController]
    public class LocationController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Location, LocationViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Location> GetQueryable()
        {
            return _context.Locations
                .Include(l => l.ParentLocation)
                .Include(l => l.Chapter)
                .Include(l => l.LanguageLocations).ThenInclude(ll => ll.Language)
                .Include(l => l.Events);
        }


        public override async Task<ActionResult<List<LocationViewModel>>> Index()
        {
            var locations = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<LocationViewModel>>(locations);
            return Ok(viewModels);
        }


        [HttpPost("create")]
        public override async Task<ActionResult<LocationViewModel>> Create([FromBody] LocationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _mapper.Map<Location>(viewModel);

            // ✅ Manually set LanguageLocation entries
            location.LanguageLocations = [.. viewModel.LanguageIds
                .Select(langId => new LanguageLocation
                {
                    LanguageId = langId
                    // EF will fill in LocationId after SaveChanges
                })];

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

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Locations.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
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
    }
}
