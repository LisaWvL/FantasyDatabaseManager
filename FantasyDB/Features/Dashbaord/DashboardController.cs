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

        [HttpGet("getDashboardGrid")]
        public async Task<IActionResult> getDashboardGrid()
        {
            try
            {
                var response = new DashboardResponse
                {
                    CalendarGrid = await _context.Dates
                        .AsNoTracking()
                        .OrderBy(d => d.Id)
                        .ToListAsync(),

                };
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
