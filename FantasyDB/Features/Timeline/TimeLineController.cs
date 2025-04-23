using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;
using FantasyDB.Features;



namespace FantasyDB.Features
{
    [ApiController]
    [Route("api/timeline")]
    public class TimelineController(AppDbContext context, IMapper mapper) : ControllerBase
    {
        private readonly BuildTimeline _builder = new(context, mapper);

        //[HttpGet("range")]
        //public async Task<ActionResult<TimelineData>> GetRange([FromQuery] int? startDateId, [FromQuery] int? endDateId)
        //{
        //    var result = await _builder.BuildAsync(startDateId, endDateId);
        //    return Ok(result);
        //}
    }
}
