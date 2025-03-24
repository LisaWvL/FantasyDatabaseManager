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
    [Route("api/event")]
    public class EventController : BaseEntityController<Event, EventViewModel>
    {

        public EventController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {

        }

        protected override IQueryable<Event> GetQueryable()
        { return _context.Event; }

        //Override Index to include Related Names
        public override async Task<IActionResult> Index()
        {
            var events = await _context.Event
                .Include(e => e.Snapshot)
                .Include(e => e.Location)
                .AsNoTracking()
                .ToListAsync();

            var viewModels = _mapper.Map<List<EventViewModel>>(events);

            ViewData["CurrentEntity"] = "Event";
            await LoadDropdownsForViewModel<EventViewModel>();

            return View("_EntityTable", viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] EventViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
            {
                return BadRequest("Invalid request");
            }

            var ev = await _context.Event.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, ev);

            _context.Event.Update(ev);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Event.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Event.Remove(ev);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event deleted" });
        }

        public override async Task<IActionResult> Create([FromBody] EventViewModel viewModel)
        {
            await LoadDropdownsForViewModel<EventViewModel>();

            return await base.Create(viewModel);
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