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
    public class PriceExampleController : Controller
    {
        private readonly AppDbContext _context;

        public PriceExampleController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prices = await _context.PriceExample.AsNoTracking().ToListAsync();

            var viewModel = prices
                .Select(p => new PriceExampleViewModel
                {
                    Id = p.Id,
                    Category = p.Category,
                    Name = p.Name,
                    Exclusivity = p.Exclusivity,
                    Price = p.Price
                })
                .ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new PriceExample());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PriceExample model)
        {
            if (ModelState.IsValid)
            {
                _context.PriceExample.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            using var reader = new StreamReader(Request.Body);
            var rawJson = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(rawJson))
                return BadRequest("No data received");

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(rawJson);
            if (data == null)
                return BadRequest("Invalid JSON");

            var price = await _context.PriceExample.FindAsync(id);
            if (price == null)
                return NotFound();

            price.Category = data.GetValueOrDefault("Category");
            price.Name = data.GetValueOrDefault("Name");
            price.Exclusivity = data.GetValueOrDefault("Exclusivity");
            price.Price = ParseNullableDecimal(data.GetValueOrDefault("Price"));

            _context.Update(price);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Price example updated successfully" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PriceExample.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.PriceExample.FindAsync(id);
            if (item != null)
            {
                _context.PriceExample.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private decimal? ParseNullableDecimal(string? value)
        {
            return decimal.TryParse(value, out var result) ? result : null;
        }
    }
}
