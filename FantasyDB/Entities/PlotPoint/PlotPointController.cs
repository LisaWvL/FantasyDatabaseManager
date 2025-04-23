//=== PlotPointController.cs ===
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities;

[Route("api/plotpoint")]
public class PlotPointController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<PlotPoint, PlotPointViewModel>(context, mapper, dropdownService)
{
    protected override IQueryable<PlotPoint> GetQueryable()
    {
        return _context.PlotPoints
            .Include(p => p.StartDate)
            .Include(p => p.EndDate)
            .Include(p => p.PlotPointChapters)
            .ThenInclude(cpp => cpp.Chapter);

    }

    public override async Task<ActionResult<List<PlotPointViewModel>>> Index()
    {
        var entities = await GetQueryable().AsNoTracking().ToListAsync();
        var viewModels = _mapper.Map<List<PlotPointViewModel>>(entities);

        var dateDict = await _context.Dates.ToDictionaryAsync(c => c.Id);
        var routeNames = await _context.Routes.ToDictionaryAsync(r => r.Id, r => r.Name);
        var riverNames = await _context.Rivers.ToDictionaryAsync(r => r.Id, r => r.Name);
        var chapterNames = await _context.ChaptersPlotPoints.ToListAsync();
        var plotpointRoutes = await _context.PlotPointsRoutes.ToListAsync();
        var plotpointRivers = await _context.PlotPointsRivers.ToListAsync();

        foreach (var vm in viewModels)
        {
            vm.StartDateName = dateDict.TryGetValue(vm.StartDateId ?? -1, out var start)
                ? $"{start.Month} {start.Day}" : null;

            vm.EndDateName = dateDict.TryGetValue(vm.EndDateId ?? -1, out var end)
                ? $"{end.Month} {end.Day}" : null;

            var matchingRoutes = plotpointRoutes
                .Where(pr => pr.PlotPointId == vm.Id)
                .Select(pr => pr.RouteId)
                .ToList();

            vm.RouteIds = matchingRoutes;
            vm.RouteNames = [.. matchingRoutes
                .Where(routeNames.ContainsKey)
                .Select(id => routeNames[id]!)];

            var matchingRivers = plotpointRivers
                .Where(pr => pr.PlotPointId == vm.Id)
                .Select(pr => pr.RiverId)
                .ToList();

            vm.RiverIds = matchingRivers;
            vm.RiverNames = [.. matchingRivers
                .Where(riverNames.ContainsKey)
                .Select(id => riverNames[id]!)];
        }

        return Ok(viewModels);
    }

    [HttpGet("{ActId}/relatedChapters")]
    public async Task<ActionResult<List<ChapterViewModel>>> GetRelatedChapters(int ActId)
    {
        var chapters = await _context.Chapters
            .Where(c => c.ActId == ActId)
            .ToListAsync();
        return chapters.Count == 0 ? NotFound() : Ok(_mapper.Map<List<ChapterViewModel>>(chapters));
    }

    [HttpGet("{BookId}/relatedActs")]
    public async Task<ActionResult<List<ActViewModel>>> GetRelatedActs(int BookId)
    {
        var acts = await _context.Acts
            .Where(a => a.BookId == BookId)
            .ToListAsync();
        return acts.Count == 0 ? NotFound() : Ok(_mapper.Map<List<ActViewModel>>(acts));
    }


    [HttpPost("{plotPointId}/add-chapter/{chapterId}")]
    public async Task<IActionResult> AddChapterToPlotPoint(int plotPointId, int chapterId)
    {
        var exists = await _context.ChaptersPlotPoints.AnyAsync(x => x.PlotPointId == plotPointId && x.ChapterId == chapterId);
        if (exists) return BadRequest("Already linked.");

        _context.ChaptersPlotPoints.Add(new ChapterPlotPoint { PlotPointId = plotPointId, ChapterId = chapterId });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{plotPointId}/remove-chapter/{chapterId}")]
    public async Task<IActionResult> RemoveChapterFromPlotPoint(int plotPointId, int chapterId)
    {
        var link = await _context.ChaptersPlotPoints.FirstOrDefaultAsync(x => x.PlotPointId == plotPointId && x.ChapterId == chapterId);
        if (link == null) return NotFound();

        _context.ChaptersPlotPoints.Remove(link);
        await _context.SaveChangesAsync();
        return Ok();
    }


    [HttpPatch("{id}/setnull")]
    public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
    {
        var entity = await _context.PlotPoints.FindAsync(id); // Change this to match the controller
        if (entity == null) return NotFound();

        var property = entity.GetType().GetProperty(fieldName);
        if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

        property.SetValue(entity, null);
        await _context.SaveChangesAsync();

        return Ok(entity);
    }


    [HttpPost("create")]
    public override async Task<ActionResult<PlotPointViewModel>> Create([FromBody] PlotPointViewModel viewModel) => await base.Create(viewModel);

    [HttpPut("{id}")]
    public override async Task<IActionResult> Update(int id, [FromBody] PlotPointViewModel viewModel) => await base.Update(id, viewModel);

    [HttpDelete("{id}")]
    public override async Task<IActionResult> Delete(int id) => await base.Delete(id);

    [HttpGet("{id}/new-chapter")]
    public override async Task<IActionResult> CreateNewChapter(int id) => await base.CreateNewChapter(id);

    [HttpGet("{id}/new-chapter-page")]
    public override async Task<IActionResult> CreateNewWritingAssistantPage(int id) => await base.CreateNewWritingAssistantPage(id);


}
