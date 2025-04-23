// 📁 Features/DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;
using System;

namespace FantasyDB.Features
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CardDesigner _designer;

        public DashboardController(AppDbContext context, CardDesigner designer)
        {
            _context = context;
            _designer = designer;
        }

        [HttpGet("getDashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var response = new DashboardResponse
                {
                    CalendarGrid = await _context.Dates
                        .AsNoTracking()
                        .OrderBy(d => d.Id)
                        .ToListAsync(),

                    Cards = new List<CardRenderResponse>()
                };

                var plotPoints = await _context.PlotPoints
                    .Include(p => p.StartDate)
                    .Include(p => p.EndDate)
                    .ToListAsync();

                var events = await _context.Events
                    .Include(e => e.StartDate)
                    .Include(e => e.EndDate)
                    .ToListAsync();

                var eras = await _context.Eras
                    .Include(e => e.StartDate)
                    .Include(e => e.EndDate)
                    .ToListAsync();

                var chapters = await _context.Chapters
                    .Include(c => c.StartDate)
                    .Include(c => c.EndDate)
                    .ToListAsync();

                var cardTasks = new List<Task<CardRenderResponse>>();

                cardTasks.AddRange(plotPoints.Select(pp => _designer.DesignCard("PlotPoint", pp, "dashboard")));
                cardTasks.AddRange(events.Select(ev => _designer.DesignCard("Event", ev, "dashboard")));
                cardTasks.AddRange(eras.Select(era => _designer.DesignCard("Era", era, "dashboard")));
                cardTasks.AddRange(chapters.Select(ch => _designer.DesignCard("Chapter", ch, "dashboard")));

                var cards = await Task.WhenAll(cardTasks);
                response.Cards.AddRange(cards);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("💥 Dashboard error: " + ex.Message);
                return StatusCode(500, "Dashboard crashed: " + ex.Message);
            }
        }

        public class DashboardResponse
        {
            public List<Date> CalendarGrid { get; set; } = new();
            public List<CardRenderResponse> Cards { get; set; } = new();
        }
    }
}
