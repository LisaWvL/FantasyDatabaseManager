using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services;
using AutoMapper;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace FantasyDBStartup.Controllers
{
    [Route("api/language")]
    public class LanguageController : BaseEntityController<Language, LanguageViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IDropdownService _dropdownService;

        public LanguageController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }


        protected override IQueryable<Language> GetQueryable() => _context.Language;

        //Override Index to include Location Name
        public override async Task<IActionResult> Index()
        {
            var languages = await _context.Language
                .Include(l => l.Locations)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<LanguageViewModel>>(languages);

            ViewData["CurrentEntity"] = "Language";
            await LoadDropdownsForViewModel<LanguageViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] LanguageViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var language = await _context.Language.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, language);

            _context.Language.Update(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var language = await _context.Language.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            _context.Language.Remove(language);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Language deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] LanguageViewModel viewModel)
        {
            await LoadDropdownsForViewModel<LanguageViewModel>();
            return await base.Create(viewModel);
        }
    }
}