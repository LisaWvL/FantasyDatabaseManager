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
    public class PriceExampleController : BaseEntityController<PriceExample, PriceExampleViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDropdownService _dropdownService;

        public PriceExampleController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<PriceExample> GetQueryable() => _context.PriceExample;

        //Override index to use Automapper
        public override async Task<IActionResult> Index()
        {
            var prices = await _context.PriceExample
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<PriceExampleViewModel>>(prices);

            ViewData["CurrentEntity"] = "PriceExample";
            // No dropdowns needed
            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] PriceExampleViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var price = await _context.PriceExample.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, price);

            _context.PriceExample.Update(price);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Price example updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var price = await _context.PriceExample.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            _context.PriceExample.Remove(price);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Price example deleted" });
        }
    }
}