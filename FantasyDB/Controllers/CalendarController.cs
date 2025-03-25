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
    public class CalendarController : BaseEntityController<Calendar, CalendarViewModel>
    {
        public CalendarController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        protected override IQueryable<Calendar> GetQueryable()
        {
            return _context.Calendar
                .Include(c => c.Event);
        }

        // -------------------------------------------------------------
        // ✅ Default GET: All calendar entries
        // -------------------------------------------------------------
        [HttpGet]
        public override async Task<ActionResult<List<CalendarViewModel>>> Index()
        {
            var entries = await GetQueryable().AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<List<CalendarViewModel>>(entries));
        }

        // -------------------------------------------------------------
        // ✅ Get by ID
        // -------------------------------------------------------------
        [HttpGet("{id}")]
        public override async Task<ActionResult<CalendarViewModel>> GetById(int id)
        {
            var day = await _context.Calendar
                .Include(c => c.Event)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (day == null)
                return NotFound();

            return Ok(_mapper.Map<CalendarViewModel>(day));
        }

        // -------------------------------------------------------------
        // ✅ Get full calendar grid (sorted by month + day)
        // -------------------------------------------------------------
        [HttpGet("grid")]
        public async Task<ActionResult<List<CalendarViewModel>>> GetFullGrid()
        {
            var all = await _context.Calendar
                .Include(c => c.Event)
                .AsNoTracking()
                .ToListAsync();

            var ordered = all
                .OrderBy(c => c.Month == "Jespen" ? 999 : GetMonthIndex(c.Month))
                .ThenBy(c => c.Day ?? 0)
                .ToList();

            return Ok(_mapper.Map<List<CalendarViewModel>>(ordered));
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
            var daysWithEvents = await _context.Calendar
                .Include(c => c.Event)
                .Where(c => c.EventId != null)
                .ToListAsync();

            return Ok(_mapper.Map<List<CalendarViewModel>>(daysWithEvents));
        }

        // -------------------------------------------------------------
        // ✅ Filter: By month
        // -------------------------------------------------------------
        [HttpGet("by-month/{month}")]
        public async Task<ActionResult<List<CalendarViewModel>>> GetByMonth(string month)
        {
            var results = await _context.Calendar
                .Include(c => c.Event)
                .Where(c => c.Month == month)
                .OrderBy(c => c.Day)
                .ToListAsync();

            return Ok(_mapper.Map<List<CalendarViewModel>>(results));
        }

        // -------------------------------------------------------------
        // ✅ Filter: Get specific day by month and day
        // -------------------------------------------------------------
        [HttpGet("day/{month}/{day}")]
        public async Task<ActionResult<CalendarViewModel>> GetByMonthDay(string month, int day)
        {
            var entry = await _context.Calendar
                .Include(c => c.Event)
                .FirstOrDefaultAsync(c => c.Month == month && c.Day == day);

            if (entry == null)
                return NotFound();

            return Ok(_mapper.Map<CalendarViewModel>(entry));
        }

        // -------------------------------------------------------------
        // ✅ Assign an Event to a specific calendar day
        // -------------------------------------------------------------
        [HttpPost("assign-event")]
        public async Task<IActionResult> AssignEvent([FromBody] AssignEventRequest request)
        {
            var calendarDay = await _context.Calendar.FindAsync(request.DayId);
            if (calendarDay == null)
                return NotFound();

            calendarDay.EventId = request.EventId;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Event assigned to calendar day." });
        }

        public class AssignEventRequest
        {
            public int DayId { get; set; }
            public int EventId { get; set; }
        }

        // -------------------------------------------------------------
        // ✅ Remove Event from a calendar day
        // -------------------------------------------------------------
        [HttpDelete("remove-event/{dayId}")]
        public async Task<IActionResult> RemoveEvent(int dayId)
        {
            var day = await _context.Calendar.FindAsync(dayId);
            if (day == null)
                return NotFound();

            day.EventId = null;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Event removed from calendar day." });
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

        // Base CRUD already handled:
        // GET /api/calendar
        // GET /api/calendar/{id}
        // POST /api/calendar/create
        // PUT /api/calendar/{id}
        // DELETE /api/calendar/{id}
    }
}
