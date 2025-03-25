using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Controllers
{
    [ApiController]
    [Route("api/route")]
    public class RouteController : BaseEntityController<Route, RouteViewModel>
    {
        public RouteController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        // ✅ Include related locations
        protected override IQueryable<Route> GetQueryable()
        {
            return _context.Routes
                .Include(r => r.From)
                .Include(r => r.To);
        }

        // ✅ Electron-ready
        public override async Task<ActionResult<List<RouteViewModel>>> Index()
        {
            var routes = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<RouteViewModel>>(routes);
            return Ok(viewModels);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<RouteViewModel>> Create([FromBody] RouteViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] RouteViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet("{id}/new-snapshot")]
        public override async Task<IActionResult> CreateNewSnapshot(int id)
        {
            return await base.CreateNewSnapshot(id);
        }

        [HttpGet("{id}/new-snapshot-page")]
        public override async Task<IActionResult> CreateNewSnapshotPage(int id)
        {
            return await base.CreateNewSnapshotPage(id);
        }
    }
}
