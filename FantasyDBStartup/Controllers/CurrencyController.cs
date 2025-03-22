using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services; // Required for IDropdownService and BaseEntityController


namespace FantasyDBStartup.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly AppDbContext _context;

        public CurrencyController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

           var currencies = await _context.Currency
                .AsNoTracking()
                .ToListAsync(); // Fetch from DB first to avoid translation errors

            var viewModel = currencies
                .AsEnumerable() // ✅ Forces client-side evaluation
                .Select(c => new CurrencyViewModel
                {
                    Id = c.Id,
                    Crown = c.Crown,
                    Shilling = c.Shilling,
                    Penny = c.Penny
                })
                .ToList();

            return View(viewModel);
        }


        public IActionResult Create()
        {
            return View(new Currency());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Currency currency)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currency);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currency = await _context.Currency.FindAsync(id);
            if (currency == null) return NotFound();

            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Currency currencyUpdate)
        {
            var currency = await _context.Currency.FindAsync(id);
            if (currency == null) return NotFound();

            currency.Crown = currencyUpdate.Crown;
            currency.Shilling = currencyUpdate.Shilling;
            currency.Penny = currencyUpdate.Penny;

            _context.Currency.Update(currency);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Currency updated successfully" });
        }
    }
}
