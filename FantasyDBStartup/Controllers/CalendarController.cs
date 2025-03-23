using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;

namespace FantasyDBStartup.Controllers
{
    [Route("api/calendar")]
    public class CalendarController : BaseEntityController<Calendar, CalendarViewModel>
    {
        private readonly IDropdownService _dropdownService;

        public CalendarController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<Calendar> GetQueryable() => _context.Calendar;

        public override async Task<IActionResult> Index()
        {
            var calendars = await _context.Calendar.ToListAsync();

            var viewModels = _mapper.Map<List<CalendarViewModel>>(calendars);
            return Ok(viewModels);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] CalendarViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var calendar = await _context.Calendar.FindAsync(id);
            if (calendar == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, calendar);

            _context.Calendar.Update(calendar);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Calendar updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var calendar = await _context.Calendar.FindAsync(id);
            if (calendar == null)
            {
                return NotFound();
            }

            _context.Calendar.Remove(calendar);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Calendar deleted" });
        }



        public override async Task<IActionResult> Create([FromBody] CalendarViewModel viewModel)
        {
            await LoadDropdownsForViewModel<CalendarViewModel>();
            return await base.Create(viewModel);
        }
    }
}