using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;

namespace FantasyDBStartup.Controllers
{
    [Route("api/character-relationship")]
    public class CharacterRelationshipController : BaseEntityController<CharacterRelationship, CharacterRelationshipViewModel>
    {
        private readonly IDropdownService _dropdownService;

        public CharacterRelationshipController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<CharacterRelationship> GetQueryable()
        {
            return _context.CharacterRelationship;
        }

        public override async Task<IActionResult> Index()
        {
            var relationships = await _context.CharacterRelationship
                .Include(r => r.Character1)
                .Include(r => r.Character2)
                .Include(r => r.Snapshot)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<CharacterRelationshipViewModel>>(relationships);

            ViewData["CurrentEntity"] = "CharacterRelationship";
            await LoadDropdownsForViewModel<CharacterRelationshipViewModel>();

            return View("_EntityTable", viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] CharacterRelationshipViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var characterRelationship = await _context.CharacterRelationship.FindAsync(id);
            if (characterRelationship == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, characterRelationship);

            _context.Update(characterRelationship);
            await _context.SaveChangesAsync();

            return Ok(new { message = "CharacterRelationship updated" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var characterRelationship = await _context.CharacterRelationship.FindAsync(id);
            if (characterRelationship == null)
            {
                return NotFound();
            }

            _context.CharacterRelationship.Remove(characterRelationship);
            await _context.SaveChangesAsync();

            return Ok(new { message = "CharacterRelationship deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] CharacterRelationshipViewModel viewModel)
        {
            await LoadDropdownsForViewModel<CharacterRelationshipViewModel>();
            return await base.Create(viewModel);
        }
    }
}