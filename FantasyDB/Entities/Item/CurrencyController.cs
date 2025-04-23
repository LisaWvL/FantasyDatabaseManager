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
    [Route("api/currency")]
    public class CurrencyController(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : BaseEntityController<Currency, CurrencyViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Currency> GetQueryable()
        {
            return _context.Currencies.AsNoTracking();
        }

        // GET: api/currency
        [HttpGet]
        public override async Task<ActionResult<List<CurrencyViewModel>>> Index()
        {
            var entities = await GetQueryable().ToListAsync();
            var viewModels = _mapper.Map<List<CurrencyViewModel>>(entities);
            return Ok(viewModels);
        }

        // GET: api/currency/{id}
        [HttpGet("{id}")]
        public override async Task<ActionResult<CurrencyViewModel>> GetById(int id)
        {
            var entity = await _context.Currencies.FindAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<CurrencyViewModel>(entity));
        }

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Currencies.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }


        // POST: api/currency/create
        [HttpPost("create")]
        public override async Task<ActionResult<CurrencyViewModel>> Create([FromBody] CurrencyViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Currency>(viewModel);
            _context.Currencies.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<CurrencyViewModel>(entity);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/currency/{id}
        [HttpPut("{id}")]
        public override async Task<IActionResult> Update(int id, [FromBody] CurrencyViewModel viewModel)
        {
            if (!ModelState.IsValid || id != viewModel.Id)
                return BadRequest();

            var entity = await _context.Currencies.FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(viewModel, entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Currency updated successfully" });
        }

        // DELETE: api/currency/{id}
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Currencies.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Currencies.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Currency deleted" });
        }

        //[HttpGet("convert")]
        //public ActionResult<decimal> ConvertCurrency(decimal amount, int fromCurrencyId, int toCurrencyId)
        //{
        //    // conversion logic
        //}

    }
}
