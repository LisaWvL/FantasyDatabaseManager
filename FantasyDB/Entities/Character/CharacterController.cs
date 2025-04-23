using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Entities
{
    [Route("api/character")]
    [ApiController]
    public class CharacterController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Character, CharacterViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Character> GetQueryable()
        {
            return _context.Characters
                .Include(c => c.Chapter)
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Language);
        }

        [HttpGet]
        public override async Task<ActionResult<List<CharacterViewModel>>> Index()
        {
            var characters = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<CharacterViewModel>>(characters);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<CharacterViewModel>> GetById(int id)
        {
            var character = await GetQueryable().FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
                return NotFound();

            var viewModel = _mapper.Map<CharacterViewModel>(character);
            return Ok(viewModel);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<CharacterViewModel>> Create([FromBody] CharacterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var character = _mapper.Map<Character>(viewModel);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            // Chapter mapping is handled by BaseEntityController (HandleChapterLinks)

            var createdViewModel = _mapper.Map<CharacterViewModel>(character);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] CharacterViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
                return NotFound();

            _mapper.Map(viewModel, character);
            await _context.SaveChangesAsync();

            await HandleJunctions(character, viewModel, isUpdate: true);
            return Ok(character);
        }

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Characters.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }


        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
                return NotFound();

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Character deleted" });
        }

        [HttpGet("{id}/new-chapter")]
        public override async Task<IActionResult> CreateNewChapter(int id)
        {
            return await base.CreateNewChapter(id);
        }

        [HttpGet("{id}/new-chapter-page")]
        public override async Task<IActionResult> CreateNewWritingAssistantPage(int id)
        {
            return await base.CreateNewWritingAssistantPage(id);
        }
    }
}
