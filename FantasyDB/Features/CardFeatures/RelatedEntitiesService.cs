// 📁 Features/CardSystem/RelatedEntitiesService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities;
using FantasyDB.Entities._Shared;
using FantasyDB.Features;
using Microsoft.Extensions.DependencyInjection;

namespace FantasyDB.Features
{
    public class RelatedEntitiesService
    {
        private readonly AppDbContext _context;
        private readonly EntitySchemaProvider _schemaProvider;
        private readonly IServiceScopeFactory _scopeFactory;


        public RelatedEntitiesService(IServiceScopeFactory scopeFactory, AppDbContext context, EntitySchemaProvider schemaProvider)
        {
            _scopeFactory = scopeFactory;
            _context = context;
            _schemaProvider = schemaProvider;
        }

        public async Task<List<RelatedEntity>> GetFlatRelatedEntities(string entityType, int entityId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var result = new List<RelatedEntity>();

            switch (entityType.ToLower())
            {
                case "plotpoint":
                    var pp = await context.PlotPoints
                        .Include(p => p.PlotPointChapters)
                            .ThenInclude(cpp => cpp.Chapter)
                        .FirstOrDefaultAsync(p => p.Id == entityId);
                    if (pp != null)
                    {
                        foreach (var cpp in pp.PlotPointChapters)
                        {
                            result.Add(new RelatedEntity
                            {
                                EntityType = "Chapter",
                                Id = cpp.Chapter.Id,
                                DisplayName = cpp.Chapter.ChapterTitle ?? $"Chapter #{cpp.Chapter.Id}"
                            });
                        }
                    }
                    break;

                case "chapter":
                    var ch = await context.Chapters
                        .Include(c => c.Act)
                        .Include(c => c.POVCharacter)
                        .FirstOrDefaultAsync(c => c.Id == entityId);
                    if (ch != null)
                    {
                        if (ch.Act != null)
                            result.Add(new RelatedEntity { EntityType = "Act", Id = ch.Act.Id, DisplayName = ch.Act.ActTitle ?? $"Act #{ch.Act.Id}" });
                        if (ch.POVCharacter != null)
                            result.Add(new RelatedEntity { EntityType = "Character", Id = ch.POVCharacter.Id, DisplayName = ch.POVCharacter.Name ?? $"Character #{ch.POVCharacter.Id}" });
                    }
                    break;

                case "event":
                    var ev = await context.Events
                        .Include(e => e.Location)
                        .FirstOrDefaultAsync(e => e.Id == entityId);
                    if (ev?.Location != null)
                    {
                        result.Add(new RelatedEntity { EntityType = "Location", Id = ev.Location.Id, DisplayName = ev.Location.Name });
                    }
                    break;

                case "era":
                    var era = await context.Eras
                        .Include(e => e.Chapter)
                        .FirstOrDefaultAsync(e => e.Id == entityId);
                    if (era?.Chapter != null)
                    {
                        result.Add(new RelatedEntity { EntityType = "Chapter", Id = era.Chapter.Id, DisplayName = era.Chapter.ChapterTitle });
                    }
                    break;
            }

            return result;
        }

        public async Task<Dictionary<int, List<RelatedEntity>>> GetRelatedEntitiesForAll(string entityType)
        {
            var results = new Dictionary<int, List<RelatedEntity>>();

            var entityTypeRuntime = _schemaProvider.TypeFromEntityType(entityType);
            var entities = (IQueryable)_context.GetType()
                .GetMethod("Set", Type.EmptyTypes)!
                .MakeGenericMethod(entityTypeRuntime)
                .Invoke(_context, null)!;

            var ids = await ((IQueryable<IEntityWithId>)entities)
                .Select(e => e.Id)
                .ToListAsync();

            var relatedTasks = ids.Select(async id => new {
                Id = id,
                Related = await GetFlatRelatedEntities(entityType, id)
            });

            var resolved = await Task.WhenAll(relatedTasks);

            foreach (var item in resolved)
            {
                results[item.Id] = item.Related;
            }

            return results;
        }


       
    }

    public interface IEntityWithId
    {
        public int Id { get; set; }
    }
}
