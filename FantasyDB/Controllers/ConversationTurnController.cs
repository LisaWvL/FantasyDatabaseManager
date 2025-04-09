using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationTurnController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<ConversationTurn, ConversationTurnViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<ConversationTurn> GetQueryable()
        {
            return _context.ConversationTurns.Include(ct => ct.PlotPoint);
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogTurn([FromBody] ConversationTurn turn)
        {
            _context.ConversationTurns.Add(turn);
            await _context.SaveChangesAsync();
            return Ok(turn);
        }

        [HttpGet("by-plotpoint/{plotpointId}")]
        public async Task<IActionResult> GetByPlotPoint(int plotpointId)
        {
            var history = await _context.ConversationTurns
                .Where(ct => ct.PlotPointId == plotpointId)
                .OrderBy(ct => ct.Timestamp)
                .ToListAsync();

            return Ok(history);
        }

        [HttpGet("latest/{plotpointId}")]
        public async Task<IActionResult> GetLatestByPlotPoint(int plotpointId, [FromQuery] int limit = 10)
        {
            var latestTurns = await _context.ConversationTurns
                .Where(ct => ct.PlotPointId == plotpointId)
                .OrderByDescending(ct => ct.Timestamp)
                .Take(limit)
                .ToListAsync();
            return Ok(latestTurns);
        }

        [HttpGet("summary/{plotpointId}")]
        public async Task<IActionResult> GetSummaryByPlotPoint(int plotpointId)
        {
            var summary = await _context.ConversationTurns
                .Where(ct => ct.PlotPointId == plotpointId && ct.IsSummary == true)
                .OrderByDescending(ct => ct.Timestamp)
                .FirstOrDefaultAsync();
            return Ok(summary);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var latestTurn = await _context.ConversationTurns
                .OrderByDescending(ct => ct.Timestamp)
                .FirstOrDefaultAsync();
            return Ok(latestTurn);
        }

        [HttpPost("summarize")]
        public async Task<IActionResult> Summarize([FromBody] ConversationTurn turn)
        {
            var summary = new ConversationTurn
            {
                Prompt = turn.Prompt,
                Response = turn.Response,
                IsSummary = true,
                Timestamp = DateTime.UtcNow,
                PlotPointId = turn.PlotPointId
            };
            _context.ConversationTurns.Add(summary);
            await _context.SaveChangesAsync();
            return Ok(summary);
        }
    }
}
