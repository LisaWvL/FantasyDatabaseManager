// 📁 Features/CardSystem/EntitySchemaProvider.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FantasyDB.Entities;
using FantasyDB.Entities._Shared;

namespace FantasyDB.Features
{
    public class EntitySchemaProvider
    {
        private readonly Dictionary<string, EntitySchema> _schemas = new(StringComparer.OrdinalIgnoreCase);

        public EntitySchemaProvider()
        {
            InitializeSchemas();
        }

        private void InitializeSchemas()
        {
            _schemas["Character"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = false },
        new SchemaField { Key = "Alias", Label = "Alias", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Role", Label = "Role", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Magic", Label = "Magic", Type = "text", Section = "header", ShowInCompact = true, Editable = true },

        new SchemaField { Key = "FactionId", Label = "Faction", Type = "fk", FkType = "Faction", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "LocationId", Label = "Location", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "LanguageId", Label = "Language", Type = "fk", FkType = "Language", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "BirthDateId", Label = "Birth Date", Type = "fk", FkType = "Date", Section = "details", ShowInCompact = false, Editable = false },

        new SchemaField { Key = "Personality", Label = "Personality", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "SocialStatus", Label = "Social Status", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Occupation", Label = "Occupation", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Desire", Label = "Desire", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Fear", Label = "Fear", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Weakness", Label = "Weakness", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "Motivation", Label = "Motivation", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Flaw", Label = "Flaw", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "Misbelief", Label = "Misbelief", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "DefiningFeatures", Label = "Defining Features", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "Gender", Label = "Gender", Type = "text", Section = "details", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "HeightCm", Label = "Height (cm)", Type = "number", Section = "details", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Build", Label = "Build", Type = "text", Section = "details", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Hair", Label = "Hair", Type = "text", Section = "details", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Eyes", Label = "Eyes", Type = "text", Section = "details", ShowInCompact = false, Editable = false }
    }
            };

            _schemas["Faction"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Alias", Label = "Alias", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Magic", Label = "Magic", Type = "text", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "FoundingYear", Label = "Founding Year", Type = "number", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "FounderId", Label = "Founder", Type = "fk", FkType = "Character", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "LeaderId", Label = "Leader", Type = "fk", FkType = "Character", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "HQLocationId", Label = "HQ Location", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Item"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Origin", Label = "Origin", Type = "text", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Effects", Label = "Effects", Type = "textarea", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "OwnerId", Label = "Owner", Type = "fk", FkType = "Character", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Language"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Type", Label = "Type", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "LocationIds", Label = "Spoken In", Type = "multiFk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Text", Label = "Text", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true }
    }
            };


            _schemas["Location"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Type", Label = "Type", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Biome", Label = "Biome", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Cultures", Label = "Cultures", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Politics", Label = "Politics", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "TotalPopulation", Label = "Population", Type = "number", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "DivineMagicians", Label = "Divine Mages", Type = "number", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "WildMagicians", Label = "Wild Mages", Type = "number", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ParentLocationId", Label = "Parent Location", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Languages", Label = "Languages Spoken", Type = "multiFk", FkType = "Language", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Events", Label = "Events", Type = "multiFk", FkType = "Event", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Event"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Description", Label = "Description", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Purpose", Label = "Purpose", Type = "text", Section = "summary", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "LocationId", Label = "Location", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "StartDateId", Label = "Start Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "EndDateId", Label = "End Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true }
    }
            };

            _schemas["PlotPoint"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Title", Label = "Title", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Description", Label = "Description", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "StartDateId", Label = "Start Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "EndDateId", Label = "End Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ChapterPlotPoints", Label = "Chapters", Type = "multiFk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Events", Label = "Events", Type = "multiFk", FkType = "Event", Section = "relation", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "PlotPointRivers", Label = "Rivers", Type = "multiFk", FkType = "River", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "PlotPointRoutes", Label = "Routes", Type = "multiFk", FkType = "Route", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Act"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "ActNumber", Label = "Act Number", Type = "number", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ActTitle", Label = "Title", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ActWordCount", Label = "Word Count", Type = "number", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "BookId", Label = "Book", Type = "fk", FkType = "Book", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Chapters", Label = "Chapters", Type = "multiFk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "ActSummary", Label = "Summary", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ActToDo", Label = "To-Do", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Book"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "BookNumber", Label = "Book Number", Type = "number", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "SeriesTitle", Label = "Series Title", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "BookTitle", Label = "Book Title", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "BookWordCount", Label = "Word Count", Type = "number", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "Acts", Label = "Acts", Type = "multiFk", FkType = "Act", Section = "relation", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "BookSummary", Label = "Summary", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "BookToDo", Label = "To-Do", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Date"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Day", Label = "Day", Type = "number", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Weekday", Label = "Weekday", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Month", Label = "Month", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Year", Label = "Year", Type = "number", Section = "header", ShowInCompact = true, Editable = true }
    }
            };
            _schemas["CharacterRelationship"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Character1Id", Label = "Character 1", Type = "fk", FkType = "Character", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Character2Id", Label = "Character 2", Type = "fk", FkType = "Character", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "RelationshipType", Label = "Type", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "RelationshipDynamic", Label = "Dynamic", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["ConversationTurn"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Prompt", Label = "Prompt", Type = "textarea", Section = "details", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "Response", Label = "Response", Type = "textarea", Section = "details", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "Role", Label = "Role", Type = "text", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "DanMode", Label = "Dan Mode", Type = "boolean", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "IsSummary", Label = "Is Summary", Type = "boolean", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "TokenCount", Label = "Token Count", Type = "number", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "SummaryLevel", Label = "Summary Level", Type = "number", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "Timestamp", Label = "Timestamp", Type = "text", Section = "summary", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "PlotPointId", Label = "PlotPoint", Type = "fk", FkType = "PlotPoint", Section = "relation", ShowInCompact = false, Editable = false },
        new SchemaField { Key = "ParentId", Label = "Parent Turn", Type = "fk", FkType = "ConversationTurn", Section = "relation", ShowInCompact = false, Editable = false }
    }
            };


            _schemas["Currency"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Crown", Label = "Crown", Type = "number", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Shilling", Label = "Shilling", Type = "number", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Penny", Label = "Penny", Type = "number", Section = "header", ShowInCompact = false, Editable = true }
    }
            };


            _schemas["Era"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "MagicSystem", Label = "Magic System", Type = "text", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "MagicStatus", Label = "Magic Status", Type = "text", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "StartDateId", Label = "Start Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "EndDateId", Label = "End Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ChapterId", Label = "Chapter", Type = "fk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "Description", Label = "Description", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true }
    }
            };


            _schemas["PriceExample"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Category", Label = "Category", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Exclusivity", Label = "Exclusivity", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Price", Label = "Price", Type = "number", Section = "summary", ShowInCompact = true, Editable = true }
    }
            };

            _schemas["River"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "DepthMeters", Label = "Depth (m)", Type = "number", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "WidthMeters", Label = "Width (m)", Type = "number", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "FlowDirection", Label = "Flow Direction", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "SourceLocationId", Label = "Source Location", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "DestinationLocationId", Label = "Destination Location", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };


            _schemas["Route"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "Name", Label = "Name", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Type", Label = "Type", Type = "text", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Length", Label = "Length (km)", Type = "number", Section = "header", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "TravelTime", Label = "Travel Time", Type = "text", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "Notes", Label = "Notes", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "FromId", Label = "From", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ToId", Label = "To", Type = "fk", FkType = "Location", Section = "relation", ShowInCompact = false, Editable = true }
    }
            };

            _schemas["Chapter"] = new EntitySchema
            {
                //PrimaryDisplay = e => $"Chapter {e.chapterNumber}: {e.chapterTitle}",
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "ChapterNumber", Label = "Chapter Number", Type = "number", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ChapterTitle", Label = "Title", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "WordCount", Label = "Word Count", Type = "number", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "ActId", Label = "Act", Type = "fk", FkType = "Act", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "POVCharacterId", Label = "POV Character", Type = "fk", FkType = "Character", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterPlotPoints", Label = "PlotPoints", Type = "multiFk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "StartDateId", Label = "Start Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "EndDateId", Label = "End Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Scenes", Label = "Scenes", Type = "multiFk", FkType = "Scene", Section = "relation", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "ChapterText", Label = "Text", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterSummary", Label = "Summary", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ToDo", Label = "To Do", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true }
    }
            };


            _schemas["Chapter"] = new EntitySchema
            {
                Fields = new List<SchemaField>
    {
        new SchemaField { Key = "ChapterNumber", Label = "Chapter Number", Type = "number", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "ChapterTitle", Label = "Title", Type = "text", Section = "header", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "WordCount", Label = "Word Count", Type = "number", Section = "header", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "ActId", Label = "Act", Type = "fk", FkType = "Act", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "POVCharacterId", Label = "POV Character", Type = "fk", FkType = "Character", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterPlotPoints", Label = "PlotPoints", Type = "multiFk", FkType = "Chapter", Section = "relation", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "StartDateId", Label = "Start Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "EndDateId", Label = "End Date", Type = "fk", FkType = "Date", Section = "relation", ShowInCompact = true, Editable = true },
        new SchemaField { Key = "Scenes", Label = "Scenes", Type = "multiFk", FkType = "Scene", Section = "relation", ShowInCompact = false, Editable = true },

        new SchemaField { Key = "ChapterText", Label = "Text", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ChapterSummary", Label = "Summary", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
        new SchemaField { Key = "ToDo", Label = "To Do", Type = "textarea", Section = "summary", ShowInCompact = false, Editable = true },
    }
            };


        }

        public List<string> GetFieldKeysToDisplay(string entityType, string context)
        {
            var schema = Get(entityType);
            return schema.Fields
                .Where(f => f.ShowIn(context))
                .Select(f => f.Key)
                .ToList();
        }

        public EntitySchema Get(string entityType) =>
            _schemas.TryGetValue(entityType, out var schema) ? schema : new EntitySchema();


        public Type TypeFromEntityType(string entityType) => entityType.ToLower() switch
        {
            "plotpoint" => typeof(PlotPoint),
            "event" => typeof(Event),
            "era" => typeof(Era),
            "chapter" => typeof(Chapter),
            "scene" => typeof(Scene),
            "character" => typeof(Character),
            "faction" => typeof(Faction),
            "item" => typeof(Item),
            "location" => typeof(Location),
            "river" => typeof(River),
            "route" => typeof(Route),
            "language" => typeof(Language),
            "date" => typeof(Date),
            "currency" => typeof(Currency),
            "characterrelationship" => typeof(CharacterRelationship),
            "conversationturn" => typeof(ConversationTurn),
            "plotpointriver" => typeof(PlotPointRiver),
            "plotpointroute" => typeof(PlotPointRoute),
            "languagelocation" => typeof(LanguageLocation),
            "priceexample" => typeof(PriceExample),
            _ => throw new Exception($"Unknown entityType: {entityType}")
        };


        public List<string> GetDropFields(string entityType, string dropContext)
        {
            return DropFieldMapping.TryGetValue(entityType, out var map) && map.TryGetValue(dropContext, out var fields)
                ? fields
                : [];
        }

        public Dictionary<string, Dictionary<string, List<string>>> DropFieldMapping { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            ["PlotPoint"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["calendar"] = new() { "StartDateId", "EndDateId" },
                ["timeline"] = new() { "StartDateId", "EndDateId" },
                ["unassigned-dropzone"] = new() { "StartDateId", "EndDateId" },
                ["context-dropzone"] = new() { "ChapterId" },
                ["river-dropzone"] = new() { "PlotPointRivers" },
                ["route-dropzone"] = new() { "PlotPointRoutes" }
            },
            ["Event"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["calendar"] = new() { "StartDateId", "EndDateId" },
                ["timeline"] = new() { "StartDateId", "EndDateId" },
                ["unassigned-dropzon"] = new() { "StartDateId", "EndDateId" },
                ["context-dropzone"] = new() { "ChapterId" },
                ["location-dropzone"] = new() { "LocationId" }
            },
            ["Chapter"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["calendar"] = new() { "StartDateId" },
                ["timeline"] = new() { "StartDateId" },
                ["unassigned-dropzon"] = new() { "StartDateId" },
                ["context-dropzone"] = new() { "StartDateId" }
            },
            ["Era"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["calendar"] = new() { "StartDateId", "EndDateId" },
                ["timeline"] = new() { "StartDateId", "EndDateId" },
                ["unassigned-dropzone"] = new() { "StartDateId", "EndDateId" }
            },
            ["Character"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["chapterEditor"] = new() { "ChapterId" },
                ["context-dropzone"] = new() { "ChapterId" },
                ["pov-dropzone"] = new() { "POVCharacterId" },
                ["unassigned-dropzone-from-context-dropzone"] = new() { "ChapterId" },
                ["unassigned-dropzone-from-pov-dropzone"] = new() { "POVCharacterId" },
                ["faction-dropzone"] = new() { "FactionId" },
                ["location-dropzone"] = new() { "LocationId" },
                ["language-dropzone"] = new() { "LanguageId" }
            },
            ["CharacterRelationship"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["context-dropzone"] = new() { "ChapterId" },
                ["unassigned-dropzone"] = new() { "Character1Id", "Character2Id" }
            },
            ["Faction"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["location-dropzone"] = new() { "HQLocationId" },
                ["unassigned-dropzone"] = new() { "HQLocationId" }
            },
            ["Item"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["context-dropzone"] = new() { "ChapterId" },
                ["owner-dropzone"] = new() { "OwnerId" },
                ["unassigned-dropzone"] = new() { "ChapterId", "OwnerId" }
            },
            ["Scene"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["chapterEditor"] = new() { "ChapterId" },
                ["unassigned-dropzone"] = new() { "ChapterId" }
            },
            ["Location"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["location-dropzone"] = new() { "ParentLocationId" },
                ["unassigned-dropzone"] = new() { "ParentLocationId" }
            },
            ["River"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["location-start"] = new() { "SourceLocationId" },
                ["location-end"] = new() { "DestinationLocationId" },
                ["unassigned-dropzone"] = new() { "SourceLocationId", "DestinationLocationId" }
            },
            ["Route"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["location-start"] = new() { "FromId" },
                ["location-end"] = new() { "ToId" },
                ["unassigned-dropzone"] = new() { "FromId", "ToId" }
            },
            ["ConversationTurn"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["context-dropzone"] = new() { "PlotPointId" },
                ["unassigned-dropzone"] = new() { "PlotPointId" }
            }
        };


        public IEnumerable<string> GetNullableFieldsOnDrop(string entityType, string context)
        {
            var schema = Get(entityType);
            var nullableFields = new List<string>();
            foreach (var field in schema.Fields)
            {
                if (field.Editable && field.ShowIn(context))
                {
                    nullableFields.Add(field.Key);
                }
            }
            return nullableFields;
        }
    }

    public class EntitySchema
    {
        public List<SchemaField> Fields { get; set; } = [];
        public Dictionary<string, List<string>> DropFieldMapping { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public class SchemaField
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public string Type { get; set; } = "";
        public string PrimaryDisplay { get; set; } = "";
        public string FkType { get; set; } = "";
        public string Section { get; set; } = "";
        public bool Editable { get; set; }
        public bool ShowInCompact { get; set; }

        public bool ShowIn(string context) =>
            context == "compact" ? ShowInCompact : true;
    }
}
        
    
