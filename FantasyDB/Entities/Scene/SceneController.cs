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

        [HttpPatch("{id}/setnull")]
        public async Task<IActionResult> SetFieldToNull(int id, [FromQuery] string fieldName)
        {
            var entity = await _context.Scenes.FindAsync(id); // Change this to match the controller
            if (entity == null) return NotFound();

            var property = entity.GetType().GetProperty(fieldName);
            if (property == null || !property.CanWrite) return BadRequest($"Field '{fieldName}' not found or not writable.");

            property.SetValue(entity, null);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }

    }
}
