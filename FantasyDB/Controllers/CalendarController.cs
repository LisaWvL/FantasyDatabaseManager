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
    [Route("api/calendar")]
    [ApiController]
    public class CalendarController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Calendar, CalendarViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Calendar> GetQueryable()

        {
            return _context.Dates.AsQueryable();

        }

        // -------------------------------------------------------------
        // ✅ Default GET: All calendar entries
        // -------------------------------------------------------------
        [HttpGet]
        public override async Task<ActionResult<List<CalendarViewModel>>> Index()
        {
            var calendars = await _context.Dates.AsNoTracking().ToListAsync();
            var events = await _context.Events.AsNoTracking().ToListAsync();

            var calendarViewModels = calendars.Select(c =>
            {
                var evt = events.FirstOrDefault(e => e.CalendarId == c.Id);
                return new CalendarViewModel
                {
                    Id = c.Id,
                    Day = c.Day,
                    Month = c.Month ?? "",
                    Weekday = c.Weekday ?? "",
                    Year = c.Year,
                };
            }).ToList();

            return Ok(calendarViewModels);
        }

        // -------------------------------------------------------------
        // ✅ Get by ID
        // -------------------------------------------------------------
        [HttpGet("{id}")]
        public override async Task<ActionResult<CalendarViewModel>> GetById(int id)
        {
            var calendar = await _context.Dates.FindAsync(id);
            if (calendar == null) return NotFound();

            var evt = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CalendarId == id);

            var viewModel = new CalendarViewModel
            {
                Id = calendar.Id,
                Day = calendar.Day,
                Month = calendar.Month ?? "",
                Weekday = calendar.Weekday ?? "",
                Year = calendar.Year,
            };

            return Ok(viewModel);
        }

        // -------------------------------------------------------------
        // ✅ Get full calendar grid (sorted by month + day)
        // -------------------------------------------------------------
        [HttpGet("grid")]
        public async Task<ActionResult<List<CalendarViewModel>>> GetFullGrid()
        {
            var calendars = await _context.Dates.AsNoTracking().ToListAsync();
            var events = await _context.Events.AsNoTracking().ToListAsync();

            var calendarViewModels = calendars
                .Select(c =>
                {
                    var evt = events.FirstOrDefault(e => e.CalendarId == c.Id);
                    return new CalendarViewModel
                    {
                        Id = c.Id,
                        Day = c.Day,
                        Month = c.Month ?? "",
                        Weekday = c.Weekday ?? "",
                        Year = c.Year,
                    };
                })
                .OrderBy(c => c.Month == "Jespen" ? 999 : GetMonthIndex(c.Month))
                .ThenBy(c => c.Day ?? 0)
                .ToList();

            return Ok(calendarViewModels);
        }

        private static int GetMonthIndex(string? month)
        {
            if (string.IsNullOrEmpty(month)) return 999;

            var months = new[]
            {
                "Aurn", "Brol", "Chana", "Drom", "Eice", "Fram", "Gila",
                "Heno", "Irrst", "Jart", "Kwarm", "Lehnd", "Moon", "Jespen"
            };

            return System.Array.IndexOf(months, month);
        }

        // -------------------------------------------------------------
        // ✅ Filter: Only days with events
        // -------------------------------------------------------------
        [HttpGet("with-events")]
        public async Task<ActionResult<List<CalendarViewModel>>> GetDaysWithEvents()
        {
            var events = await _context.Events.AsNoTracking().ToListAsync();
            var calendars = await _context.Dates
                .Where(c => events.Any(e => e.CalendarId == c.Id))
                .ToListAsync();

            var result = calendars.Select(c =>
            {
                var evt = events.FirstOrDefault(e => e.CalendarId == c.Id);
                return new CalendarViewModel
                {
                    Id = c.Id,
                    Day = c.Day,
                    Month = c.Month ?? "",
                    Weekday = c.Weekday ?? "",
                    Year = c.Year,
                };
            }).ToList();

            return Ok(result);
        }

        // -------------------------------------------------------------
        // ✅ Filter: By month
        // -------------------------------------------------------------
        [HttpGet("by-month/{month}")]
        public async Task<ActionResult<List<CalendarViewModel>>> GetByMonth(string month)
        {
            var calendars = await _context.Dates
                .Where(c => c.Month == month)
                .OrderBy(c => c.Day)
                .ToListAsync();

            var events = await _context.Events.AsNoTracking().ToListAsync();

            var result = calendars.Select(c =>
            {
                var evt = events.FirstOrDefault(e => e.CalendarId == c.Id);
                return new CalendarViewModel
                {
                    Id = c.Id,
                    Day = c.Day,
                    Month = c.Month ?? "",
                    Weekday = c.Weekday ?? "",
                    Year = c.Year,
                };
            }).ToList();

            return Ok(result);
        }

        // -------------------------------------------------------------
        // ✅ Filter: Get specific day by month and day
        // -------------------------------------------------------------
        [HttpGet("day/{month}/{day}")]
        public async Task<ActionResult<CalendarViewModel>> GetByMonthDay(string month, int day)
        {
            var calendar = await _context.Dates
                .FirstOrDefaultAsync(c => c.Month == month && c.Day == day);

            if (calendar == null) return NotFound();

            var evt = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.CalendarId == calendar.Id);

            var result = new CalendarViewModel
            {
                Id = calendar.Id,
                Day = calendar.Day,
                Month = calendar.Month ?? "",
                Weekday = calendar.Weekday ?? "",
                Year = calendar.Year,
            };

            return Ok(result);
        }

        // -------------------------------------------------------------
        // ✅ Assign an Event to a specific calendar day
        // -------------------------------------------------------------
        [HttpPost("assign-event")]
        public async Task<IActionResult> AssignEvent([FromBody] AssignEventRequest request)
        {
            var eventEntry = await _context.Events.FindAsync(request.EventId);
            if (eventEntry == null) return NotFound("Event not found.");

            eventEntry.CalendarId = request.DayId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event assigned to calendar day." });
        }

        // -------------------------------------------------------------
        // ✅ Remove Event from a calendar day
        // -------------------------------------------------------------
        [HttpDelete("remove-event/{dayId}")]
        public async Task<IActionResult> RemoveEvent(int dayId)
        {
            var evt = await _context.Events
                .FirstOrDefaultAsync(e => e.CalendarId == dayId);

            if (evt == null)
                return NotFound("No event assigned to this calendar day.");

            evt.CalendarId = 0;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event removed from calendar day." });
        }

        public class AssignEventRequest
        {
            public int DayId { get; set; }
            public int EventId { get; set; }
        }

        // -------------------------------------------------------------
        // ✅ Provide weekdays and months (from the DropdownService)
        // -------------------------------------------------------------
        [HttpGet("weekdays")]
        public async Task<IActionResult> GetWeekdays()
        {
            var weekdays = await _dropdownService.GetWeekdaysAsync();
            return Ok(weekdays);
        }

        [HttpGet("months")]
        public async Task<IActionResult> GetMonths()
        {
            var months = await _dropdownService.GetMonthsAsync();
            return Ok(months);
        }

        // -------------------------------------------------------------
        // ✅ BaseEntityController already handles:
        // GET /api/calendar
        // GET /api/calendar/{id}
        // POST /api/calendar/create
        // PUT /api/calendar/{id}
        // DELETE /api/calendar/{id}
        // -------------------------------------------------------------
    }
}