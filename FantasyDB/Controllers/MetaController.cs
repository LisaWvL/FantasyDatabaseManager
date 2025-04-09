using FantasyDB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FantasyDB.Controllers
{
    [Route("api/meta")]
    [ApiController]
    public class MetaController(IEntityRegistryService registry) : ControllerBase
    {
        private readonly IEntityRegistryService _registry = registry;

        [HttpGet("entities")]
        public IActionResult GetEntities()
        {
            var result = _registry.GetEntityMap()
                .ToDictionary(kvp => kvp.Key, kvp => new
                {
                    ModelType = kvp.Value.ModelType.Name,
                    ViewModelType = kvp.Value.ViewModelType.Name
                });

            return Ok(result);
        }
    }
}