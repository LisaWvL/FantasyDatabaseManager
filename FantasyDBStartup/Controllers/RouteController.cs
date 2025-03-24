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
    [Route("api/route")]
    public class RouteController : BaseEntityController<FantasyDB.Models.Route, RouteViewModel>
    {

        public RouteController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        protected override IQueryable<FantasyDB.Models.Route> GetQueryable()
        { return _context.Route; }

        //Override Index to include Location Names
        public override async Task<IActionResult> Index()
        {
            var routes = await _context.Route
                .Include(r => r.From)
                .Include(r => r.To)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<RouteViewModel>>(routes);

            ViewData["CurrentEntity"] = "Route";
            await LoadDropdownsForViewModel<RouteViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] RouteViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var route = await _context.Route.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, route);

            _context.Route.Update(route);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Route updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var route = await _context.Route.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            _context.Route.Remove(route);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Route deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] RouteViewModel viewModel)
        {
            await LoadDropdownsForViewModel<RouteViewModel>();

            return await base.Create(viewModel);
        }


    }
}