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
    public class CurrencyController : BaseEntityController<Currency, CurrencyViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDropdownService _dropdownService;

        public CurrencyController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<Currency> GetQueryable() => _context.Currency;

        //Override Index to include Related Names
        public override async Task<IActionResult> Index()
        {
            var currencies = await _context.Currency
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<CurrencyViewModel>>(currencies);

            ViewData["CurrentEntity"] = "Currency";
            // No dropdowns needed
            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] CurrencyViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var currency = await _context.Currency.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, currency);

            _context.Currency.Update(currency);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Currency updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var currency = await _context.Currency.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }

            _context.Currency.Remove(currency);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Currency deleted" });
        }
    }
}