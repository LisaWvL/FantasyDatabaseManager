using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Entities
{
    [Route("api/date")]
    [ApiController]
    public class DateController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Date, DateViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Date> GetQueryable()

        {
            return _context.Dates.AsQueryable();

        }

        // -------------------------------------------------------------
        // ✅ Default GET: All date entries
        // -------------------------------------------------------------
        [HttpGet]
        public override async Task<ActionResult<List<DateViewModel>>> Index()
        {
            var dates = await _context.Dates.AsNoTracking().ToListAsync();
            var events = await _context.Events.AsNoTracking().ToListAsync();

            var dateViewModels = dates.Select(c =>
            {
                var start = events.FirstOrDefault(e => e.StartDateId == c.Id);
                var end = events.FirstOrDefault(e => e.EndDateId == c.Id);
                return new DateViewModel
                {
                    Id = c.Id,
                    Day = c.Day,
                    Month = c.Month ?? "",
                    Weekday = c.Weekday ?? "",
                    Year = c.Year,
                };
            }).ToList();

            return Ok(dateViewModels);
        }

        // -------------------------------------------------------------
        // ✅ Get by ID
        // -------------------------------------------------------------
        [HttpGet("{id}")]
        public override async Task<ActionResult<DateViewModel>> GetById(int id)
        {
            var date = await _context.Dates.FindAsync(id);
            if (date == null) return NotFound();

            var evt = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.StartDateId == id);

            var viewModel = new DateViewModel
            {
                Id = date.Id,
                Day = date.Day,
                Month = date.Month ?? "",
                Weekday = date.Weekday ?? "",
                Year = date.Year,
            };

            return Ok(viewModel);
        }

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Dates.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }


        // -------------------------------------------------------------
        // ✅ Get full date grid (sorted by month + day)
        // -------------------------------------------------------------
        [HttpGet("grid")]
        public async Task<ActionResult<List<DateViewModel>>> GetFullGrid()
        {
            var dates = await _context.Dates.AsNoTracking().ToListAsync();
            var events = await _context.Events.AsNoTracking().ToListAsync();

            var dateViewModels = dates
                .Select(c =>
                {
                    var start = events.FirstOrDefault(e => e.StartDateId == c.Id);
                    var end = events.FirstOrDefault(e => e.EndDateId == c.Id);

                    return new DateViewModel
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

            return Ok(dateViewModels);
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
        // ✅ BaseEntityController already handles:
        // GET /api/date
        // GET /api/date/{id}
        // POST /api/date/create
        // PUT /api/date/{id}
        // DELETE /api/date/{id}
        // -------------------------------------------------------------
    }
}