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
    [Route("api/river")]
    public class RiverController : BaseEntityController<River, RiverViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IDropdownService _dropdownService;

        public RiverController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
            _dropdownService = dropdownService;
        }

        protected override IQueryable<River> GetQueryable() => _context.River;

        //Override Index to include Location Names
        public override async Task<IActionResult> Index()
        {
            var rivers = await _context.River
                .Include(r => r.SourceLocation)
                .Include(r => r.DestinationLocation)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<RiverViewModel>>(rivers);

            ViewData["CurrentEntity"] = "River";
            await LoadDropdownsForViewModel<RiverViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] RiverViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var river = await _context.River.FindAsync(id);
            if (river == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, river);

            _context.River.Update(river);
            await _context.SaveChangesAsync();

            return Ok(new { message = "River updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var river = await _context.River.FindAsync(id);
            if (river == null)
            {
                return NotFound();
            }

            _context.River.Remove(river);
            await _context.SaveChangesAsync();

            return Ok(new { message = "River deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] RiverViewModel viewModel)
        {
            await LoadDropdownsForViewModel<RiverViewModel>();
            return await base.Create(viewModel);
        }
    }
}