using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FantasyDB.Entities._Shared
{
    [ApiController]
    [Route("api/dropdown")]
    public class DropdownController(IDropdownService dropdownService) : ControllerBase
    {
        [HttpGet("factions")]
        public async Task<ActionResult<List<SimpleItem>>> GetFactions() =>
            Ok(await dropdownService.GetFactionsAsync());

        [HttpGet("characters")]
        public async Task<ActionResult<List<SimpleItem>>> GetCharacters() =>
            Ok(await dropdownService.GetCharactersAsync());

        [HttpGet("locations")]
        public async Task<ActionResult<List<SimpleItem>>> GetLocations() =>
            Ok(await dropdownService.GetLocationsAsync());

        [HttpGet("languages")]
        public async Task<ActionResult<List<SimpleItem>>> GetLanguages() =>
            Ok(await dropdownService.GetLanguagesAsync());

        [HttpGet("chapters")]
        public async Task<ActionResult<List<SimpleItem>>> GetChapters() =>
            Ok(await dropdownService.GetChaptersAsync());

        [HttpGet("events")]
        public async Task<ActionResult<List<SimpleItem>>> GetEvents() =>
            Ok(await dropdownService.GetEventsAsync());

        [HttpGet("eras")]
        public async Task<ActionResult<List<SimpleItem>>> GetEras() =>
            Ok(await dropdownService.GetErasAsync());

        [HttpGet("Items")]
        public async Task<ActionResult<List<SimpleItem>>> GetItems() =>
            Ok(await dropdownService.GetItemsAsync());

        [HttpGet("routes")]
        public async Task<ActionResult<List<SimpleItem>>> GetRoutes() =>
            Ok(await dropdownService.GetRoutesAsync());

        [HttpGet("rivers")]
        public async Task<ActionResult<List<SimpleItem>>> GetRivers() =>
            Ok(await dropdownService.GetRiversAsync());

        [HttpGet("weekdays")]
        public async Task<ActionResult<List<SimpleItem>>> GetWeekdays() =>
            Ok(await dropdownService.GetWeekdaysAsync());

        [HttpGet("months")]
        public async Task<ActionResult<List<SimpleItem>>> GetMonths() =>
            Ok(await dropdownService.GetMonthsAsync());

        [HttpGet("character-relationships")]
        public async Task<ActionResult<List<SimpleItem>>> GetCharacterRelationships() =>
            Ok(await dropdownService.GetCharacterRelationshipsAsync());

        [HttpGet("character-relationships/{characterId}")]
        public async Task<ActionResult<List<SimpleItem>>> GetCharacterRelationshipsForCharacter(int characterId) =>
            Ok(await dropdownService.GetCharacterRelationshipsForCharacterAsync(characterId));

        [HttpGet("price-examples")]
        public async Task<ActionResult<List<SimpleItem>>> GetPriceExamples() =>
            Ok(await dropdownService.GetPriceExamplesAsync());
        [HttpGet("plot-points")]
        public async Task<ActionResult<List<SimpleItem>>> GetPlotPoints() =>
            Ok(await dropdownService.GetPlotPointsAsync());
        [HttpGet("dates")]
        public async Task<ActionResult<List<SimpleItem>>> GetDates() =>
            Ok(await dropdownService.GetDatesAsync());
        [HttpGet("books")]
        public async Task<ActionResult<List<SimpleItem>>> GetBooks() =>
            Ok(await dropdownService.GetBooksAsync());
        [HttpGet("acts")]
        public async Task<ActionResult<List<SimpleItem>>> GetActs() =>
            Ok(await dropdownService.GetActsAsync());
        [HttpGet("scenes")]
        public async Task<ActionResult<List<SimpleItem>>> GetScenes() =>
            Ok(await dropdownService.GetScenesAsync());
    }
}
