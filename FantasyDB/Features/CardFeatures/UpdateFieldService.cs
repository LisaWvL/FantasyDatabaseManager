using System;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;

namespace FantasyDB.Features
{
    public class UpdateFieldService
    {
        private readonly AppDbContext _context;
        private readonly EntitySchemaProvider _schemaProvider;

        public UpdateFieldService(AppDbContext context, EntitySchemaProvider schemaProvider)
        {
            _context = context;
            _schemaProvider = schemaProvider;
        }

        public async Task<bool> UpdateDateRangeAsync(string entityType, int id, int? startId, int? endId)
        {
            var type = _schemaProvider.TypeFromEntityType(entityType);
            var entity = await _context.FindAsync(type, id);
            if (entity == null) return false;

            var startProp = type.GetProperty("StartDateId");
            var endProp = type.GetProperty("EndDateId");

            if (startProp != null) startProp.SetValue(entity, startId);
            if (endProp != null) endProp.SetValue(entity, endId);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
