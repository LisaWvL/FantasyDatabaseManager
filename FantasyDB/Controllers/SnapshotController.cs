using System;
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
            try
            {
                var snapshots = await GetQueryable().AsNoTracking().ToListAsync();
                Console.WriteLine($"[SnapshotController] Fetched {snapshots.Count} snapshot(s)");

                var viewModels = _mapper.Map<List<SnapshotViewModel>>(snapshots);
                Console.WriteLine($"[SnapshotController] Mapped {viewModels.Count} snapshot view models");

                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SnapshotController.Index failed: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"🔎 Inner: {ex.InnerException.Message}");
                return StatusCode(500, "Snapshot fetch failed.");
            }
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

        // ✅ NEW ENDPOINT: All connected entities for a Snapshot
        [HttpGet("{id}/entities")]
        public async Task<IActionResult> GetSnapshotEntities(int id)
        {
            var characters = await _context.Characters
                .Where(c => c.SnapshotId == id)
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Language)
                .ToListAsync();

            var items = await _context.SnapshotsItems
                .Where(si => si.SnapshotId == id)
                .Include(si => si.Item)
                .Select(si => si.Item)
                .ToListAsync();

            var locations = await _context.SnapshotsLocations
                .Where(sl => sl.SnapshotId == id)
                .Include(sl => sl.Location)
                .Select(sl => sl.Location)
                .ToListAsync();

            var eventsList = await _context.SnapshotsEvents
                .Where(se => se.SnapshotId == id)
                .Include(se => se.Event)
                .Select(se => se.Event)
                .ToListAsync();

            var factions = await _context.SnapshotsFactions
                .Where(sf => sf.SnapshotId == id)
                .Include(sf => sf.Faction)
                .Select(sf => sf.Faction)
                .ToListAsync();

            var relationships = await _context.SnapshotsCharacterRelationships
                .Where(scr => scr.SnapshotId == id)
                .Include(scr => scr.CharacterRelationship)
                .Select(scr => scr.CharacterRelationship)
                .ToListAsync();

            var eras = await _context.SnapshotsEras
                .Where(se => se.SnapshotId == id)
                .Include(se => se.Era)
                .Select(se => se.Era)
                .ToListAsync();

            return Ok(new
            {
                Characters = characters,
                Items = items,
                Locations = locations,
                Events = eventsList,
                Factions = factions,
                Relationships = relationships,
                Eras = eras
            });
        }
    }
}