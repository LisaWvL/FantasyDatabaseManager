using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Controllers;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : BaseEntityController<Event, EventViewModel>
    {
        public EventController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
            : base(context, mapper, dropdownService)
        {
        }

        protected override IQueryable<Event> GetQueryable()
        {
            return _context.Events
                .Include(e => e.Location)
                .Include(e => e.Snapshot);
        }

        // GET: api/event
        public override async Task<ActionResult<List<EventViewModel>>> Index()
        {
            var list = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<EventViewModel>>(list);
            return Ok(viewModels);
        }

        // GET: api/event/5
        [HttpGet("{id}")]
        public override async Task<ActionResult<EventViewModel>> GetById(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Snapshot)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            return Ok(_mapper.Map<EventViewModel>(ev));
        }

        // POST: api/event
        [HttpPost("create")]
        public override async Task<ActionResult<EventViewModel>> Create([FromBody] EventViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Event>(viewModel);
            _context.Events.Add(entity);
            await _context.SaveChangesAsync();

            await HandleJunctions(entity, viewModel, isUpdate: false);

            var created = _mapper.Map<EventViewModel>(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/event/5
        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] EventViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest("Mismatched IDs");

            var entity = await _context.Events.FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(viewModel, entity);
            await _context.SaveChangesAsync();

            await HandleJunctions(entity, viewModel, isUpdate: true);
            return Ok(new { message = "Event updated" });
        }

        // DELETE: api/event/5
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Events.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event deleted" });
        }

        // GET: api/event/5/new-snapshot
        [HttpGet("{id}/new-snapshot")]
        public override async Task<IActionResult> CreateNewSnapshot(int id)
        {
            return await base.CreateNewSnapshot(id);
        }

        // GET: api/event/5/new-snapshot-page
        [HttpGet("{id}/new-snapshot-page")]
        public override async Task<IActionResult> CreateNewSnapshotPage(int id)
        {
            return await base.CreateNewSnapshotPage(id);
        }
    }
}
