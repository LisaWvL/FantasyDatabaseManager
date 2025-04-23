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
    [Route("api/characterrelationship")]
    [ApiController]
    public class CharacterRelationshipController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<CharacterRelationship, CharacterRelationshipViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<CharacterRelationship> GetQueryable()
        {
            return _context.CharacterRelationships
                .Include(r => r.Character1)
                .Include(r => r.Character2)
                .Include(r => r.Chapter);
        }

        [HttpGet]
        public override async Task<ActionResult<List<CharacterRelationshipViewModel>>> Index()
        {
            var relationships = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<CharacterRelationshipViewModel>>(relationships);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<CharacterRelationshipViewModel>> GetById(int id)
        {
            var relationship = await GetQueryable().FirstOrDefaultAsync(r => r.Id == id);
            if (relationship == null)
                return NotFound();

            var viewModel = _mapper.Map<CharacterRelationshipViewModel>(relationship);
            return Ok(viewModel);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<CharacterRelationshipViewModel>> Create([FromBody] CharacterRelationshipViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<CharacterRelationship>(viewModel);
            _context.CharacterRelationships.Add(entity);
            await _context.SaveChangesAsync();

            await HandleJunctions(entity, viewModel, isUpdate: false);

            var createdViewModel = _mapper.Map<CharacterRelationshipViewModel>(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] CharacterRelationshipViewModel viewModel)
        {
            var relationship = await _context.CharacterRelationships.FindAsync(viewModel.Id);
            if (relationship == null)
                return NotFound();
            _context.CharacterRelationships.Remove(relationship);
            await _context.SaveChangesAsync();
            return Ok(new { message = "CharacterRelationship deleted" });
        }


        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] CharacterRelationshipViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            var relationship = await _context.CharacterRelationships.FindAsync(id);
            if (relationship == null)
                return NotFound();

            _mapper.Map(viewModel, relationship);
            await _context.SaveChangesAsync();

            await HandleJunctions(relationship, viewModel, isUpdate: true);
            return Ok(relationship);
        }


        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.CharacterRelationships.FindAsync(id); // Change this to match the controller
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
            var relationship = await _context.CharacterRelationships.FindAsync(id);
            if (relationship == null)
                return NotFound();

            _context.CharacterRelationships.Remove(relationship);
            await _context.SaveChangesAsync();

            return Ok(new { message = "CharacterRelationship deleted" });
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

        [HttpGet("filter/by-character/{characterId}")]
        public async Task<IActionResult> GetByCharacterId(int characterId)
        {
            var rels = await _context.CharacterRelationships
                .Where(r => r.Character1Id == characterId || r.Character2Id == characterId)
                .Include(r => r.Character1)
                .Include(r => r.Character2)
                .Include(r => r.Chapter)
                .ToListAsync();

            return Ok(_mapper.Map<List<CharacterRelationshipViewModel>>(rels));
        }
    }
}
