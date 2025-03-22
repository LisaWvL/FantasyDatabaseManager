using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services; // Required for IDropdownService and BaseEntityController

namespace FantasyDBStartup.Controllers
{
    public class CalendarController : Controller
    {
        private readonly AppDbContext _context;

        public CalendarController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var calendars = await _context.Calendar
                .AsNoTracking()
                .ToListAsync(); // Fetch from DB first to avoid translation errors

            var viewModel = calendars
                .AsEnumerable() // ✅ Forces client-side evaluation
                .Select(c => new CalendarViewModel
                {
                    Id = c.Id,
                    Weekdays = c.Weekdays,
                    Months = c.Months,
                    DaysPerWeek = c.DaysPerWeek,
                    MonthsPerYear = c.MonthsPerYear,
                    DaysPerYear = c.DaysPerYear,
                    EventId = c.EventId,
                    EventName = _context.Event.Where(e => e.Id == c.EventId).Select(e => e.Name).FirstOrDefault()
                })
                .ToList();

            await LoadDropdowns();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new Calendar());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadDropdowns();
            return View(calendar);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var calendar = await _context.Calendar.FindAsync(id);
            if (calendar == null) return NotFound();

            await LoadDropdowns();
            return View(calendar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Calendar calendarUpdate)
        {
            var calendar = await _context.Calendar.FindAsync(id);
            if (calendar == null) return NotFound();

            calendar.Weekdays = calendarUpdate.Weekdays;
            calendar.Months = calendarUpdate.Months;
            calendar.DaysPerWeek = calendarUpdate.DaysPerWeek;
            calendar.MonthsPerYear = calendarUpdate.MonthsPerYear;
            calendar.DaysPerYear = calendarUpdate.DaysPerYear;
            calendar.EventId = calendarUpdate.EventId;

            _context.Calendar.Update(calendar);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Calendar updated successfully" });
        }

        private async Task LoadDropdowns()
        {
            ViewData["EventIdList"] = new SelectList(await _context.Event.ToListAsync(), "Id", "Name");
        }
    }
}
