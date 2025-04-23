using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities;
using FantasyDB.Entities._Shared;

namespace FantasyDB.Features.Timeline
{ }
public class BuildTimeline
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BuildTimeline(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TimelineData> BuildAsync(int? startDateId = null, int? endDateId = null)
    {
        var calendar = await _context.Dates.OrderBy(d => d.Id).ToListAsync();
        if (!startDateId.HasValue) startDateId = calendar.FirstOrDefault()?.Id;
        if (!endDateId.HasValue) endDateId = calendar.LastOrDefault()?.Id;

        var data = new TimelineData();

        data.PlotPoints = await _context.PlotPoints
            .Where(p => p.StartDateId >= startDateId && p.StartDateId <= endDateId ||
                        p.EndDateId >= startDateId && p.EndDateId <= endDateId)
            .Select(p => new TimelineEventVM
            {
                Id = p.Id,
                EntityType = "PlotPoint",
                Name = p.Title,
                StartDateId = p.StartDateId,
                EndDateId = p.EndDateId
            }).ToListAsync();

        data.Chapters = await _context.Chapters
            .Where(p => p.StartDateId >= startDateId && p.StartDateId <= endDateId ||
                        p.EndDateId >= startDateId && p.EndDateId <= endDateId)
            .Select(p => new TimelineEventVM
            {
                Id = p.Id,
                EntityType = "Chapter",
                Name = p.ChapterTitle,
                StartDateId = p.StartDateId,
                EndDateId = p.EndDateId
            }).ToListAsync();

        data.Eras = await _context.Eras
            .Where(p => p.StartDateId >= startDateId && p.StartDateId <= endDateId ||
                        p.EndDateId >= startDateId && p.EndDateId <= endDateId)
            .Select(p => new TimelineEventVM
            {
                Id = p.Id,
                EntityType = "Era",
                Name = p.Name,
                StartDateId = p.StartDateId,
                EndDateId = p.EndDateId
            }).ToListAsync();

        data.Events = await _context.Events
            .Where(p => p.StartDateId >= startDateId && p.StartDateId <= endDateId ||
                        p.EndDateId >= startDateId && p.EndDateId <= endDateId)
            .Select(p => new TimelineEventVM
            {
                Id = p.Id,
                EntityType = "Event",
                Name = p.Name,
                StartDateId = p.StartDateId,
                EndDateId = p.EndDateId
            }).ToListAsync();

        return data;
    }
}
