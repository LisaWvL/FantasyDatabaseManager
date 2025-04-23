// 📁 Features/CardSystem/DropHandlerService.cs

using System.Linq;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;

namespace FantasyDB.Features
{
    public class DropHandlerService
    {
        private readonly AppDbContext _context;
        private readonly EntitySchemaProvider _schemaProvider;

        public DropHandlerService(AppDbContext context, EntitySchemaProvider schemaProvider)
        {
            _context = context;
            _schemaProvider = schemaProvider;
        }

        // 🧠 Handles logic for dropping a card into a target context/component
        public async Task<object> ProcessDropAsync(DropPayload payload)
        {
            var type = _schemaProvider.TypeFromEntityType(payload.EntityType);
            var entity = await _context.FindAsync(type, payload.Id);
            if (entity == null)
                return new { success = false, message = "Entity not found." };

            var dropFields = _schemaProvider.GetDropFields(payload.EntityType, payload.DropTargetType);
            if (!dropFields.Any())
                return new { success = false, message = "No drop rules for this drop target." };

            foreach (var field in dropFields)
            {
                var prop = type.GetProperty(field);
                if (prop != null && prop.CanWrite)
                    prop.SetValue(entity, payload.DropTargetId);
            }

            await _context.SaveChangesAsync();
            return new { success = true, message = $"Updated: {string.Join(", ", dropFields)}" };
        }

        // 🧹 Clears values when dropped to unassigned sidebar
        public async Task<object> ClearFieldsOnUnassignedDrop(DropToUnassignedRequest request)
        {
            var type = _schemaProvider.TypeFromEntityType(request.EntityType);
            var entity = await _context.FindAsync(type, request.Id);
            if (entity == null)
                return new { success = false, message = "Entity not found." };

            var dropFields = _schemaProvider.GetDropFields(request.EntityType, request.FromContext);
            if (!dropFields.Any())
                return new { success = false, message = "No fields to clear for this context." };

            foreach (var field in dropFields)
            {
                var prop = type.GetProperty(field);
                if (prop != null && prop.CanWrite)
                    prop.SetValue(entity, null);
            }

            await _context.SaveChangesAsync();
            return new { success = true, message = $"Cleared: {string.Join(", ", dropFields)}" };
        }
    }
}
