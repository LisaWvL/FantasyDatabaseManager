using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using FantasyDB.Services; // Required for IDropdownService and BaseEntityController


namespace FantasyDBStartup.Controllers
{
    public class CharacterRelationshipController : BaseEntityController<CharacterRelationship, CharacterRelationshipViewModel>
    {
        public CharacterRelationshipController(AppDbContext context, IDropdownService dropdowns)
            : base(context, dropdowns)
        {
        }

        protected override IQueryable<CharacterRelationship> GetQueryable()
        {
            return _context.CharacterRelationship.AsNoTracking();
        }

        protected override CharacterRelationshipViewModel MapToViewModel(CharacterRelationship r)
        {
            return new CharacterRelationshipViewModel
            {
                Id = r.Id,
                Character1Id = r.Character1Id,
                Character2Id = r.Character2Id,
                RelationshipType = r.RelationshipType,
                RelationshipDynamic = r.RelationshipDynamic,
                SnapshotId = r.SnapshotId,

                Character1Name = _context.Character.FirstOrDefault(c => c.Id == r.Character1Id)?.Name,
                Character2Name = _context.Character.FirstOrDefault(c => c.Id == r.Character2Id)?.Name,
                SnapshotName = _context.Snapshot.FirstOrDefault(s => s.Id == r.SnapshotId)?.SnapshotName
            };
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            using var reader = new StreamReader(Request.Body);
            var rawJson = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(rawJson))
                return BadRequest("No data received");

            var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(rawJson);
            if (data == null) return BadRequest("Invalid JSON");

            var relationship = await _context.CharacterRelationship.FindAsync(id);
            if (relationship == null) return NotFound();

            relationship.Character1Id = ParseNullableInt(data.GetValueOrDefault("Character1Id"));
            relationship.Character2Id = ParseNullableInt(data.GetValueOrDefault("Character2Id"));
            relationship.SnapshotId = ParseNullableInt(data.GetValueOrDefault("SnapshotId"));
            relationship.RelationshipType = data.GetValueOrDefault("RelationshipType");
            relationship.RelationshipDynamic = data.GetValueOrDefault("RelationshipDynamic");

            _context.CharacterRelationship.Update(relationship);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Character relationship updated" });
        }

        private int? ParseNullableInt(string? value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}
