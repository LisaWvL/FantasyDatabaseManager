using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace FantasyDB.Entities._Shared
{


    [ApiController]
    [Route("api/flat")]
    public class FlatEntityController(AppDbContext context) : ControllerBase
    {
        [HttpGet("{entityType}")]
        public async Task<IActionResult> GetFlatByType(string entityType)
        {
            var lowerType = entityType.ToLowerInvariant();

            switch (lowerType)
            {
                case "character":
                    return Ok(await context.Characters.AsNoTracking().ToListAsync());
                case "scene":
                    return Ok(await context.Scenes.AsNoTracking().ToListAsync());
                case "chapter":
                    return Ok(await context.Chapters.AsNoTracking().ToListAsync());
                case "act":
                    return Ok(await context.Acts.AsNoTracking().ToListAsync());
                case "book":
                    return Ok(await context.Books.AsNoTracking().ToListAsync());
                case "date":
                    return Ok(await context.Dates.AsNoTracking().ToListAsync());
                case "currency":
                    return Ok(await context.Currencies.AsNoTracking().ToListAsync());
                case "language":
                    return Ok(await context.Languages.AsNoTracking().ToListAsync());
                case "location":
                    return Ok(await context.Locations.AsNoTracking().ToListAsync());
                case "plotpoint":
                    return Ok(await context.PlotPoints.AsNoTracking().ToListAsync());
                case "river":
                    return Ok(await context.Rivers.AsNoTracking().ToListAsync());
                case "route":
                    return Ok(await context.Routes.AsNoTracking().ToListAsync());
                case "faction":
                    return Ok(await context.Factions.AsNoTracking().ToListAsync());
                case "item":
                    return Ok(await context.Items.AsNoTracking().ToListAsync());
                case "event":
                    return Ok(await context.Events.AsNoTracking().ToListAsync());
                case "era":
                    return Ok(await context.Eras.AsNoTracking().ToListAsync());
                case "characterrelationship":
                    var relationships = await context.CharacterRelationships
                        .Include(r => r.Character1)
                        .Include(r => r.Character2)
                        .AsNoTracking()
                        .Select(r => new
                        {
                            r.Id,
                            r.Character1Id,
                            r.Character2Id,
                            Name1 = r.Character1.Name,
                            Name2 = r.Character2.Name,
                            r.RelationshipDynamic,
                            r.RelationshipType,
                            r.ChapterId,
                        })
                        .ToListAsync();
                    return Ok(relationships);
                default:
                    return BadRequest($"Unknown entity type: {entityType}");
            }
        }
    }
}
