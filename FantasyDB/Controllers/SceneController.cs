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
    [Route("api/scene")]
    public class SceneController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
     : BaseEntityController<Scene, SceneViewModel>(context, mapper, dropdownService)
    {
        protected override IQueryable<Scene> GetQueryable()
        {
            return _context.Scenes
                .Include(s => s.Chapter);
        }

        [HttpPost("{chapterId}/reorder-scenes")]
        public async Task<IActionResult> ReorderScenes(int chapterId, [FromBody] List<int> orderedSceneIds)
        {
            var scenes = await _context.Scenes
                .Where(s => s.ChapterId == chapterId)
                .ToListAsync();

            for (int i = 0; i < orderedSceneIds.Count; i++)
            {
                var scene = scenes.FirstOrDefault(s => s.Id == orderedSceneIds[i]);
                if (scene != null)
                    scene.SceneNumber = i + 1;
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }
}
