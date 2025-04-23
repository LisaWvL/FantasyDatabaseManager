// 📁 Features/CardSystem/CardSystem.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities;
using FantasyDB.Entities._Shared;
using System.Linq;


namespace FantasyDB.Features
{

    public class CardDesigner
    {
        private readonly AppDbContext _context;
        private readonly RelatedEntitiesService _relatedEntitiesService;
        private readonly EntitySchemaProvider _schemaProvider;

        public CardDesigner(AppDbContext context, RelatedEntitiesService relatedEntitiesService, EntitySchemaProvider schemaProvider)
        {
            _context = context;
            _relatedEntitiesService = relatedEntitiesService;
            _schemaProvider = schemaProvider;
        }

        public static Type TypeFromEntityType(string entityType) => entityType.ToLower() switch
        {
            "plotpoint" => typeof(PlotPoint),
            "event" => typeof(Event),
            "era" => typeof(Era),
            "chapter" => typeof(Chapter),
            "character" => typeof(Character),
            "item" => typeof(Item),
            "location" => typeof(Location),
            "faction" => typeof(Faction),
            "language" => typeof(Language),
            "scene" => typeof(Scene),
            "river" => typeof(River),
            "route" => typeof(Route),
            "book" => typeof(Book),
            "act" => typeof(Act),
            _ => throw new ArgumentException($"Unknown entity type: {entityType}")
        };





        public async Task<CardRenderResponse> DesignCard(string entityType, object entity, string context)
        {
            if (entity == null)
            {
                Console.WriteLine("⚠️ DesignCard: entity is null, returning empty response.");
                return null!;
            }

            var idProp = entity.GetType().GetProperty("Id");
            if (idProp == null)
            {
                Console.WriteLine("⚠️ DesignCard: entity does not have an 'Id' property, returning empty response.");
                return null!;
            }

            int id = (int)(idProp.GetValue(entity) ?? throw new InvalidOperationException("Id property is null"));
            var schema = _schemaProvider.Get(entityType);

            var entityTypeProperties = entity.GetType().GetProperties();
            var displayFields = new Dictionary<string, object>();

            Console.WriteLine($"🧩 DesignCard for {entityType} ({id}) in context: {context}");

            var displayKeys = _schemaProvider.GetFieldKeysToDisplay(entityType, context);

            foreach (var key in displayKeys)
            {
                var prop = entityTypeProperties.FirstOrDefault(p =>
                    string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    displayFields[key] = prop.GetValue(entity);
                }
            }
            displayFields["Id"] = id;
            displayFields["EntityType"] = entityType;

            Console.WriteLine($"🧪 DisplayFields for {entityType} #{id}: " + string.Join(", ", displayFields.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            return new CardRenderResponse
            {
                Id = id,
                EntityType = entityType,
                DisplayMode = GetDisplayMode(context),
                Styling = await GetStyling(entityType, entity),
                RelatedEntities = await _relatedEntitiesService.GetFlatRelatedEntities(entityType, id),
                FkOptions = new(),
                TargetZone = GetTargetZone(entityType, entity),
                CardData = displayFields
            };
        }

        private static string GetDisplayMode(string context) => context.ToLower() switch
        {
            "dashboard" => "compact",
            "unassignedSidebar" => "compact",
            "section" => "basic",
            "calendar" => "compact",
            "edit" => "full",
            "create" => "full",
            "clone" => "full",
            "detail" => "full",
            _ => "basic"
        };

        private static string GetTargetZone(string entityType, object entity)
        {
            var start = entity.GetType().GetProperty("StartDateId")?.GetValue(entity) as int?;
            return start.HasValue ? $"calendarCell_{start}" : "unassigned";
        }

        private async Task<Dictionary<string, object>> GetStyling(string entityType, object entity)
        {
            var colorMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Character"] = "#a368ff",
                ["Chapter"] = "#ff8a65",
                ["Era"] = "#ffb74d",
                ["Scene"] = "#4fc3f7",
                ["Faction"] = "#ffb74d",
                ["Item"] = "#81c784",
                ["Event"] = "#f06292",
                ["Location"] = "#64b5f6",
                ["Language"] = "#7986cb",
                ["PlotPoint"] = "#9575cd",
                ["CharacterRelationship"] = "#ffd54f",
                ["Act"] = "#4db6ac",
                ["Book"] = "#4db6ac",
                ["Date"] = "#4db6ac",
                ["Currency"] = "#4db6ac",
                ["Route"] = "#4db6ac",
                ["River"] = "#4db6ac",
                ["PriceExample"] = "#4db6ac",
                ["ConversationTurn"] = "#4db6ac"
            };

            var styling = new Dictionary<string, object>
            {
                ["color"] = colorMap.TryGetValue(entityType, out var hex) ? hex : "#888"
            };

            int? start = entity.GetType().GetProperty("StartDateId")?.GetValue(entity) as int?;
            int? end = entity.GetType().GetProperty("EndDateId")?.GetValue(entity) as int?;

            if (start != null && end != null)
            {
                styling["isSpanning"] = start != end;
                styling["isReversed"] = await IsDateBefore(end.Value, start.Value);
            }

            return styling;
        }


        private async Task<bool> IsDateBefore(int startDateId, int endDateId)
        {
            var startDate = await _context.Dates.AsNoTracking().FirstOrDefaultAsync(d => d.Id == startDateId);
            var endDate = await _context.Dates.AsNoTracking().FirstOrDefaultAsync(d => d.Id == endDateId);

            if (startDate == null || endDate == null)
                return false;

            // Alphabetical month order
            int startMonthIndex = string.IsNullOrWhiteSpace(startDate.Month) ? 13 : startDate.Month[0];
            int endMonthIndex = string.IsNullOrWhiteSpace(endDate.Month) ? 13 : endDate.Month[0];

            // Compare year → month → day
            if (startDate.Year < endDate.Year)
                return true;
            if (startDate.Year > endDate.Year)
                return false;

            if (string.IsNullOrEmpty(startDate.Month) && !string.IsNullOrEmpty(endDate.Month))
                return false; // YearEndDay is after all months
            if (!string.IsNullOrEmpty(startDate.Month) && string.IsNullOrEmpty(endDate.Month))
                return true;  // Normal day is before YearEndDay

            if (string.Compare(startDate.Month, endDate.Month, StringComparison.Ordinal) < 0)
                return true;
            if (string.Compare(startDate.Month, endDate.Month, StringComparison.Ordinal) > 0)
                return false;

            return startDate.Day < endDate.Day;
        }

    }

    public class CardRenderResponse
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public object CardData { get; set; } = default!;
        public string DisplayMode { get; set; } = "basic";
        public Dictionary<string, object> Styling { get; set; } = new();
        public List<RelatedEntity> RelatedEntities { get; set; } = new();
        public List<DropdownOption> FkOptions { get; set; } = new();
        public string TargetZone { get; set; } = "default";
    }

    public class RelatedEntity
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = "";
        public string DisplayName { get; set; } = "";
    }

    public class DropdownOption
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = "";
    }

    public class LoadAndRender
    {
        public async Task<List<CardRenderResponse>> Load(string entityType, string context)
        {
            // dummy fallback — this was the old implementation
            return new List<CardRenderResponse>();
        }
    }

    public class DropPayload
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string DropTargetType { get; set; } = string.Empty; // "calendar", "unassigned", etc.
        public int? DropTargetId { get; set; }
        public string Context { get; set; } = string.Empty;
    }

    public class DropToUnassignedRequest
    {
        public string EntityType { get; set; } = string.Empty;
        public int Id { get; set; }
        public string? FromContext { get; set; } // optional
    }

    public class DateRangeUpdateRequest
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public int? StartDateId { get; set; }
        public int? EndDateId { get; set; }
    }

}