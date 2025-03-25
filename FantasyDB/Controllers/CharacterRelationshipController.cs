using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Controllers
{
    [Route("api/characterrelationship")]
    [ApiController]
    public class CharacterRelationshipController : BaseEntityController<CharacterRelationship, CharacterRelationshipViewModel>
    {
        public CharacterRelationshipController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        protected override IQueryable<CharacterRelationship> GetQueryable()
        {
            return _context.CharacterRelationships
                .Include(r => r.Character1)
                .Include(r => r.Character2)
                .Include(r => r.Snapshot);
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

        [HttpGet("{id}/new-snapshot")]
        public override async Task<IActionResult> CreateNewSnapshot(int id)
        {
            return await base.CreateNewSnapshot(id);
        }

        [HttpGet("{id}/new-snapshot-page")]
        public override async Task<IActionResult> CreateNewSnapshotPage(int id)
        {
            return await base.CreateNewSnapshotPage(id);
        }

        [HttpGet("filter/by-character/{characterId}")]
        public async Task<IActionResult> GetByCharacterId(int characterId)
        {
            var rels = await _context.CharacterRelationships
                .Where(r => r.Character1Id == characterId || r.Character2Id == characterId)
                .Include(r => r.Character1)
                .Include(r => r.Character2)
                .Include(r => r.Snapshot)
                .ToListAsync();

            return Ok(_mapper.Map<List<CharacterRelationshipViewModel>>(rels));
        }
    }
}
