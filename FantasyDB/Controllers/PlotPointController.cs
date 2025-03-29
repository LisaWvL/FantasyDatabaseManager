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
            .Include(p => p.StartDate)
            .Include(p => p.endDate)
            .Include(p => p.Snapshot);
    }

    public override async Task<ActionResult<List<PlotPointViewModel>>> Index()
    {
        var entities = await GetQueryable().AsNoTracking().ToListAsync();
        var viewModels = _mapper.Map<List<PlotPointViewModel>>(entities);
        var calendarDict = _context.Calendar.ToDictionary(c => c.Id, c => c);


        // Fill readable names
        foreach (var vm in viewModels)
        {
            vm.StartDateName = calendarDict.TryGetValue(vm.StartDateId ?? -1, out var start)
                ? $"{start.Month} {start.Day}" : null;
            vm.EndDateName = calendarDict.TryGetValue(vm.EndDateId ?? -1, out var end)
                ? $"{end.Month} {end.Day}" : null;
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

    [HttpGet("by-calendar/{startDateId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> ByCalendar(int startDateId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.startDateId == startDateId)
            .ToListAsync();
        var vms = _mapper.Map<List<PlotPointViewModel>>(plotPoints);
        return Ok(vms);
    }

    [HttpGet("by-calendar-range")]
    public async Task<ActionResult<List<PlotPointViewModel>>> ByCalendarRange(int startDateId, int endDateId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.startDateId == startDateId && p.endDateId == endDateId)
            .ToListAsync();
        var vms = _mapper.Map<List<PlotPointViewModel>>(plotPoints);
        return Ok(vms);
    }



    [HttpGet("by-snapshot/{snapshotId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> BySnapshot(int snapshotId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.SnapshotId == snapshotId)
            .ToListAsync();
        var vms = _mapper.Map<List<PlotPointViewModel>>(plotPoints);
        return Ok(vms);
    }

}
