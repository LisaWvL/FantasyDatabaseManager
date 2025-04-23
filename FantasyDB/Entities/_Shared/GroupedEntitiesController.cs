using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyDB.Entities._Shared;

[ApiController]
[Route("api/grouped")]
public class GroupedEntitiesController(AppDbContext context, IMapper mapper) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;


    [HttpGet("{id}/{entityType}")]
    public async Task<ActionResult<object>> GetGrouped(int id, string entityType)
    {
        entityType = entityType.ToLower();

        switch (entityType)
        {
            case "scene":
                var scene = await _context.Scenes
                    .Include(s => s.Chapter)
                        .ThenInclude(ch => ch.Act)
                            .ThenInclude(a => a.Book)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (scene == null) return NotFound();

                return Ok(new
                {
                    Scene = _mapper.Map<SceneViewModel>(scene),
                    Chapter = scene.Chapter != null ? _mapper.Map<ChapterViewModel>(scene.Chapter) : null,
                    Act = scene.Chapter?.Act != null ? _mapper.Map<ActViewModel>(scene.Chapter.Act) : null,
                    Book = scene.Chapter?.Act?.Book != null ? _mapper.Map<BookViewModel>(scene.Chapter.Act.Book) : null
                });

            case "chapter":
                var chapter = await _context.Chapters
                    .Include(ch => ch.Act)
                        .ThenInclude(a => a.Book)
                    .Include(ch => ch.Scenes)
                    .FirstOrDefaultAsync(ch => ch.Id == id);

                if (chapter == null) return NotFound();

                return Ok(new
                {
                    Chapter = _mapper.Map<ChapterViewModel>(chapter),
                    Act = chapter.Act != null ? _mapper.Map<ActViewModel>(chapter.Act) : null,
                    Book = chapter.Act?.Book != null ? _mapper.Map<BookViewModel>(chapter.Act.Book) : null,
                    Scenes = chapter.Scenes.Select(s => _mapper.Map<SceneViewModel>(s)).ToList()
                });

            case "act":
                var act = await _context.Acts
                    .Include(a => a.Book)
                    .Include(a => a.Chapters)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (act == null) return NotFound();

                return Ok(new
                {
                    Act = _mapper.Map<ActViewModel>(act),
                    Book = act.Book != null ? _mapper.Map<BookViewModel>(act.Book) : null,
                    Chapters = act.Chapters.Select(c => _mapper.Map<ChapterViewModel>(c)).ToList()
                });

            case "book":
                var book = await _context.Books
                    .Include(b => b.Acts)
                        .ThenInclude(a => a.Chapters)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null) return NotFound();

                return Ok(new
                {
                    Book = _mapper.Map<BookViewModel>(book),
                    Acts = book.Acts.Select(a => new
                    {
                        Act = _mapper.Map<ActViewModel>(a),
                        Chapters = a.Chapters.Select(c => _mapper.Map<ChapterViewModel>(c)).ToList()
                    }).ToList()
                });

            default:
                return BadRequest("Unsupported entity type.");
        }
    }

    [HttpGet("{startDateId}/{endDateId}/range")]
    public async Task<IActionResult> GetEntitiesInDateRange(int startDateId, int endDateId)
    {
        // Load all calendar dates between the IDs
        var calendarIds = await _context.Dates
            .Where(d => d.Id >= startDateId && d.Id <= endDateId)
            .Select(d => d.Id)
            .ToListAsync();

        var plotPoints = await _context.PlotPoints
            .Where(pp => calendarIds.Contains(pp.StartDateId ?? -1) || calendarIds.Contains(pp.EndDateId ?? -1))
            .Include(pp => pp.StartDate)
            .Include(pp => pp.EndDate)
            .ToListAsync();

        var chapters = await _context.Chapters
            .Where(ch => calendarIds.Contains(ch.StartDateId ?? -1) || calendarIds.Contains(ch.EndDateId ?? -1))
            .Include(ch => ch.StartDate)
            .Include(ch => ch.EndDate)
            .ToListAsync();

        var eras = await _context.Eras
            .Where(e => calendarIds.Contains(e.StartDateId ?? -1) || calendarIds.Contains(e.EndDateId ?? -1))
            .Include(e => e.StartDate)
            .Include(e => e.EndDate)
            .ToListAsync();

        var events = await _context.Events
            .Where(ev => calendarIds.Contains(ev.StartDateId ?? -1) || calendarIds.Contains(ev.EndDateId ?? -1))
            .Include(ev => ev.StartDate)
            .Include(ev => ev.EndDate)
            .ToListAsync();

        return Ok(new
        {
            StartDateId = startDateId,
            EndDateId = endDateId,
            PlotPoints = _mapper.Map<List<PlotPointViewModel>>(plotPoints),
            Chapters = _mapper.Map<List<ChapterViewModel>>(chapters),
            Eras = _mapper.Map<List<EraViewModel>>(eras),
            Events = _mapper.Map<List<EventViewModel>>(events)
        });
    }
}

