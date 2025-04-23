//// File: Controllers/LoadAndRenderController.cs
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using FantasyDB.Features;

//namespace FantasyDB.Entities._Shared
//{
//    [ApiController]
//    [Route("api/load")]
//    public class LoadAndRenderController : ControllerBase
//    {
//        private readonly LoadAndRender _loadAndRender;

//        public LoadAndRenderController(LoadAndRender loadAndRender)
//        {
//            _loadAndRender = loadAndRender;
//        }

//        [HttpGet("dashboard")]
//        public async Task<IActionResult> LoadCards([FromQuery] string entityType, [FromQuery] string context)
//        {
//            var cards = await _loadAndRender.Load(entityType, context);
//            return Ok(cards);
//        }
//    }
//}
