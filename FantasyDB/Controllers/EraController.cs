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
    [ApiController]
    [Route("api/era")]
    public class EraController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Era, EraViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Era> GetQueryable()
        {
            return _context.Eras.Include(e => e.Chapter).AsNoTracking();
        }

        [HttpGet]
        public override async Task<ActionResult<List<EraViewModel>>> Index()
        {
            var entities = await GetQueryable().ToListAsync();
            var viewModels = _mapper.Map<List<EraViewModel>>(entities);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<EraViewModel>> GetById(int id)
        {
            var entity = await _context.Eras
                .Include(e => e.Chapter)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<EraViewModel>(entity));
        }

        [HttpPost("create")]
        public override async Task<ActionResult<EraViewModel>> Create([FromBody] EraViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = _mapper.Map<Era>(viewModel);
            _context.Eras.Add(model);
            await _context.SaveChangesAsync();

            // Handle chapter entry
            await HandleChapterLinks(model, viewModel, model.Id);

            var result = _mapper.Map<EraViewModel>(model);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] EraViewModel viewModel)
        {
            if (!ModelState.IsValid || id != viewModel.Id)
                return BadRequest(ModelState);

            var entity = await _context.Eras.FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(viewModel, entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Era updated successfully" });
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Eras.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Eras.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Era deleted" });
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
