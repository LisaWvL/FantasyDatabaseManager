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
    }
}
