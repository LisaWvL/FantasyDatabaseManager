using Microsoft.AspNetCore.Mvc;
using System.Linq;

[Route("api/meta")]
[ApiController]
public class MetaController : ControllerBase
{
    private readonly IEntityRegistryService _registry;

    public MetaController(IEntityRegistryService registry)
    {
        _registry = registry;
    }

    [HttpGet("entities")]
    public IActionResult GetEntities()
    {
        var result = _registry.GetEntityMap()
            .ToDictionary(kvp => kvp.Key, kvp => new
            {
                ModelType = kvp.Value.Item1.Name,
                ViewModelType = kvp.Value.Item2.Name
            });

        return Ok(result);
    }
}
