using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDBStartup.Controllers
{
    [Route("api/character")]
    public class CharacterController : BaseEntityController<Character, CharacterViewModel>
    {
        // NOTE: You don't need to redeclare _dropdownService if you're using it in the base class only.
        // But you can keep it here if this controller needs to use dropdowns in custom endpoints.
        private readonly IDropdownService _dropdownService;


        public CharacterController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        // Override Index to include related entities so dropdown names resolve in your table
        public override async Task<IActionResult> Index()
        {
            var characters = await _context.Character
                .Include(c => c.Snapshot)
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Language)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<CharacterViewModel>>(characters);

            ViewData["CurrentEntity"] = "Character";
            await LoadDropdownsForViewModel<CharacterViewModel>();

            return View("_EntityTable", viewModels); // Assuming you're using the shared table partial
        }

        // Optional: render a character details page
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id)
        {
            var character = await _context.Character
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Snapshot)
                .Include(c => c.Language)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound();

            var viewModel = _mapper.Map<CharacterViewModel>(character);
            return Ok(viewModel);
        }

        // Optional: return a Razor partial or full view of a character's info
        [HttpGet("{id}/details")]
        public async Task<IActionResult> Details(int id)
        {
            var character = await _context.Character
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Snapshot)
                .Include(c => c.Language)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound();

            var viewModel = _mapper.Map<CharacterViewModel>(character);

            return View("Details", viewModel); // optional Details.cshtml view
        }

        // Render the form for new character creation
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            ViewData["CurrentEntity"] = "Character";
            await LoadDropdownsForViewModel<CharacterViewModel>();
            return View("Create");
        }

        // POST: Create new character via form or inline-editing
        [HttpPost("create")]
        public override async Task<IActionResult> Create([FromBody] CharacterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var character = _mapper.Map<Character>(viewModel);
                _context.Character.Add(character);
                await _context.SaveChangesAsync();

                if (character.SnapshotId.HasValue)
                {
                    var snapshotCharacter = new SnapshotCharacter
                    {
                        CharacterId = character.Id,
                        SnapshotId = character.SnapshotId.Value
                    };

                    _context.SnapshotCharacter.Add(snapshotCharacter);
                    await _context.SaveChangesAsync();
                }


                var createdViewModel = _mapper.Map<CharacterViewModel>(character);
                return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
            }
            if (!ModelState.IsValid)
            {
                var errors = string.Join("\n", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest("Invalid Model: \n" + errors);
            }

            return BadRequest(ModelState);
        }

        // PUT: Update via inline-edit or form (optional View)
        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] CharacterViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        // DELETE: Remove character
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        // Optional: Render a Razor edit page (not just inline)
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
                return NotFound();

            var viewModel = _mapper.Map<CharacterViewModel>(character);
            await LoadDropdownsForViewModel<CharacterViewModel>();
            return View("Edit", viewModel);
        }

        [HttpGet("{id}/new-snapshot")]
        public override async Task<IActionResult> CreateNewSnapshot(int id)
        {
            return await base.CreateNewSnapshot(id);
        }


        protected override IQueryable<Character> GetQueryable()
        {
            return _context.Character
                .Include(c => c.Snapshot)
                .Include(c => c.Faction)
                .Include(c => c.Location)
                .Include(c => c.Language);
        }


    }
}