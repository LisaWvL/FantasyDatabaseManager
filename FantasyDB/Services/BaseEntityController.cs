using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantasyDB.Services
{
    public abstract class BaseEntityController<TModel, TViewModel> : Controller
        where TModel : class, new()
    {
        protected readonly AppDbContext _context;
        protected readonly IDropdownService _dropdowns;

        protected BaseEntityController(AppDbContext context, IDropdownService dropdowns)
        {
            _context = context;
            _dropdowns = dropdowns;
        }

        protected abstract IQueryable<TModel> GetQueryable();

        protected abstract TViewModel MapToViewModel(TModel entity);

        public virtual async Task<IActionResult> Index()
        {
            var entities = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = entities.Select(MapToViewModel).ToList();
            await PopulateDropdowns(); // ✅ Ensures dropdowns load before rendering
            return View(viewModels);
        }

        public virtual async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new TModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(TModel entity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await PopulateDropdowns();
            return View(entity);
        }
        protected virtual async Task PopulateDropdowns()
        {
            ViewData["FactionIdList"] = await _dropdowns.GetFactionsAsync();
            ViewData["Character1IdList"] = await _dropdowns.GetCharactersAsync();
            ViewData["Character2IdList"] = await _dropdowns.GetCharactersAsync(); // For CharacterRelationship
            ViewData["LocationIdList"] = await _dropdowns.GetLocationsAsync();
            ViewData["LanguageIdList"] = await _dropdowns.GetLanguagesAsync();
            ViewData["SnapshotIdList"] = await _dropdowns.GetSnapshotsAsync();
            ViewData["EventIdList"] = await _dropdowns.GetEventsAsync();
            ViewData["EraIdList"] = await _dropdowns.GetErasAsync();
        }

    }
}
