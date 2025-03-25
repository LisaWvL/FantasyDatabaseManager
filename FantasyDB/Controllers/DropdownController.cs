using System.Collections.Generic;
using System.Threading.Tasks;
using FantasyDB.Interfaces;
using FantasyDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyDB.Controllers
{
    [ApiController]
    [Route("api/dropdown")]
    public class DropdownController : ControllerBase
    {
        private readonly IDropdownService _dropdownService;

        public DropdownController(IDropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        [HttpGet("factions")]
        public async Task<ActionResult<List<SimpleItem>>> GetFactions() =>
            Ok(await _dropdownService.GetFactionsAsync());

        [HttpGet("characters")]
        public async Task<ActionResult<List<SimpleItem>>> GetCharacters() =>
            Ok(await _dropdownService.GetCharactersAsync());

        [HttpGet("locations")]
        public async Task<ActionResult<List<SimpleItem>>> GetLocations() =>
            Ok(await _dropdownService.GetLocationsAsync());

        [HttpGet("languages")]
        public async Task<ActionResult<List<SimpleItem>>> GetLanguages() =>
            Ok(await _dropdownService.GetLanguagesAsync());

        [HttpGet("snapshots")]
        public async Task<ActionResult<List<SimpleItem>>> GetSnapshots() =>
            Ok(await _dropdownService.GetSnapshotsAsync());

        [HttpGet("events")]
        public async Task<ActionResult<List<SimpleItem>>> GetEvents() =>
            Ok(await _dropdownService.GetEventsAsync());

        [HttpGet("eras")]
        public async Task<ActionResult<List<SimpleItem>>> GetEras() =>
            Ok(await _dropdownService.GetErasAsync());

        [HttpGet("artifacts")]
        public async Task<ActionResult<List<SimpleItem>>> GetArtifacts() =>
            Ok(await _dropdownService.GetArtifactsAsync());

        [HttpGet("routes")]
        public async Task<ActionResult<List<SimpleItem>>> GetRoutes() =>
            Ok(await _dropdownService.GetRoutesAsync());

        [HttpGet("rivers")]
        public async Task<ActionResult<List<SimpleItem>>> GetRivers() =>
            Ok(await _dropdownService.GetRiversAsync());

        [HttpGet("weekdays")]
        public async Task<ActionResult<List<SimpleItem>>> GetWeekdays() =>
            Ok(await _dropdownService.GetWeekdaysAsync());

        [HttpGet("months")]
        public async Task<ActionResult<List<SimpleItem>>> GetMonths() =>
            Ok(await _dropdownService.GetMonthsAsync());

        [HttpGet("character-relationships")]
        public async Task<ActionResult<List<SimpleItem>>> GetCharacterRelationships() =>
            Ok(await _dropdownService.GetCharacterRelationshipsAsync());

        [HttpGet("character-relationships/{characterId}")]
        public async Task<ActionResult<List<SimpleItem>>> GetCharacterRelationshipsForCharacter(int characterId) =>
            Ok(await _dropdownService.GetCharacterRelationshipsForCharacterAsync(characterId));
    }
}
