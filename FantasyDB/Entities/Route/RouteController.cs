using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace FantasyDB.Entities
{
    [ApiController]
    [Route("api/route")]
    public class RouteController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Route, RouteViewModel>(context, mapper, dropdownService)
    {

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

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Routes.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }


        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet("{id}/new-chapter")]
        public override async Task<IActionResult> CreateNewChapter(int id)
        {
            return await base.CreateNewChapter(id);
        }

        [HttpGet("{id}/new-chapter-page")]
        public override async Task<IActionResult> CreateNewWritingAssistantPage(int id)
        {
            return await base.CreateNewWritingAssistantPage(id);
        }
    }
}
