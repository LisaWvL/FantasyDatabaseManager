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
    [Route("api/priceexample")]
    public class PriceExampleController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<PriceExample, PriceExampleViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<PriceExample> GetQueryable()
        {
            return _context.PriceExamples;
        }

        public override async Task<ActionResult<List<PriceExampleViewModel>>> Index()
        {
            var prices = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<PriceExampleViewModel>>(prices);
            return Ok(viewModels);
        }

        [HttpPost("create")]
        public override async Task<ActionResult<PriceExampleViewModel>> Create([FromBody] PriceExampleViewModel viewModel)
        {
            return await base.Create(viewModel);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] PriceExampleViewModel viewModel)
        {
            return await base.Update(id, viewModel);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }


        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.PriceExamples.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

    }
}
