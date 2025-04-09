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
public class PlotPointController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<PlotPoint, PlotPointViewModel>(context, mapper, dropdownService)
{
    protected override IQueryable<PlotPoint> GetQueryable()
    {
        return _context.PlotPoints
            .Include(p => p.StartDate)
            .Include(p => p.endDate)
            .Include(p => p.Chapter);
    }

    public override async Task<ActionResult<List<PlotPointViewModel>>> Index()
    {
        var entities = await GetQueryable().AsNoTracking().ToListAsync();
        var viewModels = _mapper.Map<List<PlotPointViewModel>>(entities);

        var calendarDict = await _context.Dates.ToDictionaryAsync(c => c.Id);

        var routeNames = await _context.Routes
            .ToDictionaryAsync(r => r.Id, r => r.Name);

        var riverNames = await _context.Rivers
            .ToDictionaryAsync(r => r.Id, r => r.Name);

        var plotpointRoutes = await _context.PlotPointsRoutes
            .ToListAsync();

        var plotpointRivers = await _context.PlotPointsRivers
            .ToListAsync();

        foreach (var vm in viewModels)
        {
            // ✅ Date names
            vm.StartDateName = calendarDict.TryGetValue(vm.StartDateId ?? -1, out var start)
                ? $"{start.Month} {start.Day}" : null;

            vm.EndDateName = calendarDict.TryGetValue(vm.EndDateId ?? -1, out var end)
                ? $"{end.Month} {end.Day}" : null;

            // ✅ Routes
            var matchingRoutes = plotpointRoutes
                .Where(pr => pr.PlotPointId == vm.Id)
                .Select(pr => pr.RouteId)
                .ToList();

            vm.RouteIds = matchingRoutes;
            vm.RouteNames = [.. matchingRoutes
                .Where(routeNames.ContainsKey)
                .Select(id => routeNames[id])
                .Where(name => name != null)
                .Select(name => name!)];

            // ✅ Rivers
            var matchingRivers = plotpointRivers
                .Where(pr => pr.PlotPointId == vm.Id)
                .Select(pr => pr.RiverId)
                .ToList();


            vm.RiverIds = matchingRivers;
            vm.RiverNames = [.. matchingRivers
                .Where(riverNames.ContainsKey)
                .Select(id => riverNames[id])
                .Where(name => name != null)
                .Select(name => name!)];
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

    // Optional: filtered by chapter
    [HttpGet("chapter/{chapterId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetByChapter(int chapterId)
    {
        var plotPoints = await GetQueryable()
            .Where(p => p.ChapterId == chapterId)
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



    [HttpGet("by-chapter/{chapterId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> ByChapter(int chapterId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.ChapterId == chapterId)
            .ToListAsync();
        var vms = _mapper.Map<List<PlotPointViewModel>>(plotPoints);
        return Ok(vms);
    }

    // ✅ NEW ENDPOINT: All connected entities for a PlotPoints
    [HttpGet("{id}/PlotPointentities")]
    public IActionResult GetPlotPointEntities(int id)
    {
        var plotPoint = _context.PlotPoints.FirstOrDefault(p => p.Id == id);
        if (plotPoint == null)
            return NotFound();

        // Find connected rivers via junction table
        var riverIds = _context.PlotPointsRivers
            .Where(link => link.PlotPointId == id)
            .Select(link => link.RiverId)
            .ToList();

        var rivers = _context.Rivers
            .Where(r => riverIds.Contains(r.Id))
            .Select(r => new { r.Id, r.Name })
            .ToList();

        // Find connected routes via junction table
        var routeIds = _context.PlotPointsRoutes
            .Where(link => link.PlotPointId == id)
            .Select(link => link.RouteId)
            .ToList();

        var routes = _context.Routes
            .Where(r => routeIds.Contains(r.Id))
            .Select(r => new { r.Id, r.Name })
            .ToList();

        var startDate = _context.Dates
            .Where(d => d.Id == plotPoint.startDateId)
            .Select(d => new { d.Day, d.Month, d.Weekday })
            .FirstOrDefault();

        var endDate = _context.Dates
            .Where(d => d.Id == plotPoint.endDateId)
            .Select(d => new { d.Day, d.Month, d.Weekday })
            .FirstOrDefault();

        return Ok(new
        {
            rivers,
            routes,
            startDate,
            endDate
        });
    }


    [HttpGet("dropdown")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetDropdown()
    {
        var plotPoints = await _context.PlotPoints
            .Select(p => new PlotPointViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDateId = p.startDateId,
                EndDateId = p.endDateId,
                ChapterId = p.ChapterId
            })
            .ToListAsync();
        return Ok(plotPoints);
    }
    [HttpGet("dropdown/{chapterId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetDropdownByChapter(int chapterId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.ChapterId == chapterId)
            .Select(p => new PlotPointViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDateId = p.startDateId,
                EndDateId = p.endDateId,
                ChapterId = p.ChapterId
            })
            .ToListAsync();
        return Ok(plotPoints);
    }
    [HttpGet("dropdown/plotpoint/{plotPointId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetDropdownByPlotPoint(int plotPointId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.Id == plotPointId)
            .Select(p => new PlotPointViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDateId = p.startDateId,
                EndDateId = p.endDateId,
                ChapterId = p.ChapterId
            })
            .ToListAsync();
        return Ok(plotPoints);
    }
    [HttpGet("dropdown/plotpoint/{plotPointId}/chapter/{chapterId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetDropdownByPlotPointAndChapter(int plotPointId, int chapterId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.Id == plotPointId && p.ChapterId == chapterId)
            .Select(p => new PlotPointViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDateId = p.startDateId,
                EndDateId = p.endDateId,
                ChapterId = p.ChapterId
            })
            .ToListAsync();
        return Ok(plotPoints);
    }
    [HttpGet("dropdown/plotpoint/{plotPointId}/chapter/{chapterId}/startdate/{startDateId}")]
    public async Task<ActionResult<List<PlotPointViewModel>>> GetDropdownByPlotPointAndChapterAndStartDate(int plotPointId, int chapterId, int startDateId)
    {
        var plotPoints = await _context.PlotPoints
            .Where(p => p.Id == plotPointId && p.ChapterId == chapterId && p.startDateId == startDateId)
            .Select(p => new PlotPointViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDateId = p.startDateId,
                EndDateId = p.endDateId,
                ChapterId = p.ChapterId
            })
            .ToListAsync();
        return Ok(plotPoints);
    }
    [HttpGet("dropdown/plotpoint/{plotPointId}/chapter/{chapterId}/startdate/{startDateId}/enddate/{endDateId}")]
    public ActionResult<List<PlotPointViewModel>> GetDropdownByPlotPointAndChapterAndStartDateAndEndDate(int plotPointId, int chapterId, int startDateId, int endDateId)
    {
        var plotPoints = _context.PlotPoints
            .Where(p => p.Id == plotPointId && p.ChapterId == chapterId && p.startDateId == startDateId && p.endDateId == endDateId)
            .Select(p => new PlotPointViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDateId = p.startDateId,
                EndDateId = p.endDateId,
                ChapterId = p.ChapterId
            })
            .ToList();
        return Ok(plotPoints);
    }
}
