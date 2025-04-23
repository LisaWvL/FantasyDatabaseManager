// 📁 Features/CardSystem/CardRenderController.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities;
using FantasyDB.Entities._Shared;

namespace FantasyDB.Features
{
    [ApiController]
    [Route("api/cards")]
    public class CardRenderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CardDesigner _designer;
        private readonly RelatedEntitiesService _relatedEntitiesService;
        private readonly UpdateFieldService _updateField;
        private readonly DropHandlerService _dropHandler;

        public CardRenderController(
            AppDbContext context,
            CardDesigner designer,
            RelatedEntitiesService relatedEntitiesService,
            UpdateFieldService updateField,
            DropHandlerService dropHandler
        )
        {
            _context = context;
            _designer = designer;
            _relatedEntitiesService = relatedEntitiesService;
            _updateField = updateField;
            _dropHandler = dropHandler;
        }

        [HttpGet("{entityType}/{id}")]
        public async Task<IActionResult> GetCard(string entityType, int id, [FromQuery] string context = "default")
        {
            var entityTypeResolved = CardDesigner.TypeFromEntityType(entityType);
            var entity = await _context.FindAsync(entityTypeResolved, id);

            if (entity is null)
                return NotFound($"No entity of type '{entityType}' with ID {id} was found.");

            var card = await _designer.DesignCard(entityType, entity, context);
            return Ok(card);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardCards()
        {
            var allCards = new List<CardRenderResponse>();

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
            allCards.AddRange(cards);

            return Ok(new { cards = allCards });
        }

        [HttpPost("drop")]
        public async Task<IActionResult> HandleDrop([FromBody] DropPayload payload)
        {
            var result = await _dropHandler.ProcessDropAsync(payload);
            return Ok(result);
        }

        [HttpPut("updateDateRange")]
        public async Task<IActionResult> UpdateDateRange([FromBody] DateRangeUpdateRequest request)
        {
            var success = await _updateField.UpdateDateRangeAsync(request.EntityType, request.Id, request.StartDateId, request.EndDateId);
            if (!success)
                return NotFound(new { message = $"Failed to update dates for {request.EntityType} with ID {request.Id}" });

            return Ok(new { message = "Date range updated successfully." });
        }

        [HttpPut("dropToUnassigned")]
        public async Task<IActionResult> DropToUnassigned([FromBody] DropToUnassignedRequest request)
        {
            var result = await _dropHandler.ClearFieldsOnUnassignedDrop(request);
            return Ok(result);
        }
    }
}



//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using FantasyDB.Entities;
//using FantasyDB.Entities._Shared;

//namespace FantasyDB.Features
//{
//    [ApiController]
//    [Route("api/cards")]
//    public class CardRenderController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly CardDesigner _designer;
//        private readonly LoadAndRender _loader;
//        private readonly RelatedEntitiesService _relatedEntitiesService;
//        private readonly UpdateFieldService _updateField;
//        private readonly DropHandlerService _dropHandler;

//        public CardRenderController(
//            AppDbContext context,
//            CardDesigner designer,
//            LoadAndRender loader,
//            RelatedEntitiesService relatedEntitiesService,
//            UpdateFieldService updateField,
//            DropHandlerService dropHandler
//        )
//        {
//            _context = context;
//            _designer = designer;
//            _loader = loader;
//            _relatedEntitiesService = relatedEntitiesService;
//            _updateField = updateField;
//            _dropHandler = dropHandler;
//        }


//        [HttpGet("{entityType}/{id}")]
//        public async Task<IActionResult> GetCard(string entityType, int id, [FromQuery] string context = "default")
//        {
//            var entityTypeResolved = CardDesigner.TypeFromEntityType(entityType);
//            var entity = await _context.FindAsync(entityTypeResolved, id);

//            if (entity is null)
//                return NotFound($"No entity of type '{entityType}' with ID {id} was found.");

//            var card = await _designer.DesignCard(entityType, entity, context);
//            return Ok(card);
//        }

//        [HttpGet("dashboard")]
//        public async Task<IActionResult> GetDashboardCards()
//        {
//            var allCards = new List<CardRenderResponse>();

//            foreach (var type in new[] { "PlotPoint", "Event", "Era", "Chapter" })
//            {
//                var cards = await _loader.Load(type, "dashboard");
//                allCards.AddRange(cards);
//            }

//            return Ok(new { cards = allCards });
//        }

//        // ⬇️ Called when a card is dropped onto a calendar, sidebar, or section
//        [HttpPost("drop")]
//        public async Task<IActionResult> HandleDrop([FromBody] DropPayload payload)
//        {
//            var result = await _dropHandler.ProcessDropAsync(payload);
//            return Ok(result);
//        }

//        // ⬇️ Called when resizing a card across dates in the calendar
//        [HttpPut("updateDateRange")]
//        public async Task<IActionResult> UpdateDateRange([FromBody] DateRangeUpdateRequest request)
//        {
//            var success = await _updateField.UpdateDateRangeAsync(request.EntityType, request.Id, request.StartDateId, request.EndDateId);
//            if (!success)
//                return NotFound(new { message = $"Failed to update dates for {request.EntityType} with ID {request.Id}" });

//            return Ok(new { message = "Date range updated successfully." });
//        }

//        // ⬇️ Alternative drop logic for clearing values
//        [HttpPut("dropToUnassigned")]
//        public async Task<IActionResult> DropToUnassigned([FromBody] DropToUnassignedRequest request)
//        {
//            var result = await _dropHandler.ClearFieldsOnUnassignedDrop(request);
//            return Ok(result);
//        }

//    }
//}
