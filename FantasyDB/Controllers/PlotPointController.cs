using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Controllers;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FantasyDB.Controllers;

[Route("api/plotpoint")]
public class PlotPointController : BaseEntityController<PlotPoint, PlotPointViewModel>
{
    public PlotPointController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
        : base(context, mapper, dropdownService)
    {
    }

    protected override IQueryable<PlotPoint> GetQueryable()
    {
        return _context.PlotPoints
            .Include(p => p.Calendar)
            .Include(p => p.Snapshot)
            .Include(p => p.PlotPointCharacters).ThenInclude(pc => pc.Character)
            .Include(p => p.PlotPointLocations).ThenInclude(pl => pl.Location)
            .Include(p => p.PlotPointEvents).ThenInclude(pe => pe.Event)
            .Include(p => p.PlotPointFactions).ThenInclude(pf => pf.Faction);
    }

    public override async Task<ActionResult<List<PlotPointViewModel>>> Index()
    {
        var entities = await GetQueryable().AsNoTracking().ToListAsync();
        var viewModels = _mapper.Map<List<PlotPointViewModel>>(entities);

        // Fill readable names
        foreach (var vm in viewModels)
        {
            vm.CalendarLabel = _context.Calendar.FirstOrDefault(c => c.Id == vm.CalendarId)?.Month + " " + _context.Calendar.FirstOrDefault(c => c.Id == vm.CalendarId)?.Day;
            vm.CharacterNames = _context.Characters.Where(c => vm.CharacterIds.Contains(c.Id)).Select(c => c.Name).ToList();
            vm.LocationNames = _context.Locations.Where(l => vm.LocationIds.Contains(l.Id)).Select(l => l.Name).ToList();
            vm.EventNames = _context.Events.Where(e => vm.EventIds.Contains(e.Id)).Select(e => e.Name).ToList();
            vm.FactionNames = _context.Factions.Where(f => vm.FactionIds.Contains(f.Id)).Select(f => f.Name).ToList();
        }

        return Ok(viewModels);
    }

    [HttpPost("create")]
    public override async Task<ActionResult<PlotPointViewModel>> Create([FromBody] PlotPointViewModel viewModel)
    {
        return await base.Create(viewModel);
    }

    [HttpPut("{id}")]
    public override async Task<IActionResult> Update(int id, [FromBody] PlotPointViewModel viewModel)
    {
        return await base.Update(id, viewModel);
    }

    [HttpDelete("{id}")]
    public override async Task<IActionResult> Delete(int id)
    {
        return await base.Delete(id);
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

    // Optional: filtered by snapshot
    [HttpGet("snapshot/{snapshotId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetBySnapshot(int snapshotId)
    {
        var plotPoints = await GetQueryable()
            .Where(p => p.SnapshotId == snapshotId)
            .ToListAsync();

        var vms = _mapper.Map<List<PlotPointViewModel>>(plotPoints);
        return Ok(vms);
    }
}
