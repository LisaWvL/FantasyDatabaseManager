using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Attributes;
using FantasyDB.Interfaces;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseEntityController<TModel, TViewModel> : ControllerBase
    where TModel : class, new()
    where TViewModel : class, IViewModelWithId
{
    protected readonly AppDbContext _context;
    protected readonly IMapper _mapper;
    protected readonly IDropdownService _dropdownService;

    protected BaseEntityController(AppDbContext context, IMapper mapper, IDropdownService dropdownService)
    {
        _context = context;
        _mapper = mapper;
        _dropdownService = dropdownService;
    }

    protected abstract IQueryable<TModel> GetQueryable();

    [HttpGet]
    public virtual async Task<ActionResult<List<TViewModel>>> Index()
    {
        var entities = await GetQueryable().AsNoTracking().ToListAsync();
        return Ok(_mapper.Map<List<TViewModel>>(entities));
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TViewModel>> GetById(int id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<TViewModel>(entity));
    }

    [HttpPost("create")]
    public virtual async Task<ActionResult<TViewModel>> Create([FromBody] TViewModel viewModel)
    {
        var entity = _mapper.Map<TModel>(viewModel);
        _context.Add(entity);
        await _context.SaveChangesAsync();

        await HandleJunctions(entity, viewModel, false);

        var result = _mapper.Map<TViewModel>(entity);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] TViewModel viewModel)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity == null) return NotFound();

        _mapper.Map(viewModel, entity);
        await _context.SaveChangesAsync();

        await HandleJunctions(entity, viewModel, true);
        return Ok(_mapper.Map<TViewModel>(entity));
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity == null) return NotFound();

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Entity deleted" });
    }

    // ========== 🔁 Handle Junctions ==========

    protected async Task HandleJunctions(TModel entity, TViewModel viewModel, bool isUpdate)
    {
        var entityId = (int)(typeof(TModel).GetProperty("Id")?.GetValue(entity) ?? 0);

        await HandleSnapshotLinks(entity, viewModel, entityId);

        foreach (var prop in typeof(TViewModel).GetProperties())
        {
            var junctionAttr = prop.GetCustomAttribute<HandlesJunctionAttribute>();
            if (junctionAttr == null) continue;

            var selectedIds = prop.GetValue(viewModel) as IEnumerable<int>;
            if (selectedIds == null) continue;

            var junctionType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == junctionAttr.JunctionEntity);
            if (junctionType == null) continue;

            var junctionSet = _context.GetType().GetMethod("Set", Type.EmptyTypes)!
                .MakeGenericMethod(junctionType)
                .Invoke(_context, null);
            if (junctionSet is not IQueryable junctionQuery) continue;

            var allJunctions = junctionQuery.Cast<object>().ToList();
            var toRemove = allJunctions
                .Where(j =>
                {
                    var thisKeyValue = j.GetType().GetProperty(junctionAttr.ThisKey)?.GetValue(j);
                    return thisKeyValue != null && (int)thisKeyValue == entityId;
                })
                .ToList();

            foreach (var j in toRemove)
                _context.Remove(j);

            foreach (var foreignId in selectedIds)
            {
                var newJunction = Activator.CreateInstance(junctionType);
                if (newJunction == null) continue;

                junctionType.GetProperty(junctionAttr.ThisKey)?.SetValue(newJunction, entityId);
                junctionType.GetProperty(junctionAttr.ForeignKey)?.SetValue(newJunction, foreignId);
                _context.Add(newJunction);
            }
        }

        await _context.SaveChangesAsync();
    }

    protected async Task HandleSnapshotLinks(TModel entity, TViewModel viewModel, int entityId)
    {
        var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
        if (snapshotProp == null) return;

        var snapshotId = snapshotProp.GetValue(viewModel) as int?;
        if (!snapshotId.HasValue) return;

        if (typeof(TModel) == typeof(Character))
            _context.SnapshotsCharacters.Add(new SnapshotCharacter { CharacterId = entityId, SnapshotId = snapshotId.Value });
        else if (typeof(TModel) == typeof(Item))
            _context.SnapshotsItems.Add(new SnapshotItem { ItemId = entityId, SnapshotId = snapshotId.Value });
        else if (typeof(TModel) == typeof(Location))
            _context.SnapshotsLocations.Add(new SnapshotLocation { LocationId = entityId, SnapshotId = snapshotId.Value });
        else if (typeof(TModel) == typeof(Faction))
            _context.SnapshotsFactions.Add(new SnapshotFaction { FactionId = entityId, SnapshotId = snapshotId.Value });
        else if (typeof(TModel) == typeof(Era))
            _context.SnapshotsEras.Add(new SnapshotEra { EraId = entityId, SnapshotId = snapshotId.Value });
        else if (typeof(TModel) == typeof(Event))
            _context.SnapshotsEvents.Add(new SnapshotEvent { EventId = entityId, SnapshotId = snapshotId.Value });
        else if (typeof(TModel) == typeof(CharacterRelationship))
            _context.SnapshotsCharacterRelationships.Add(new SnapshotCharacterRelationship { CharacterRelationshipId = entityId, SnapshotId = snapshotId.Value });

        await _context.SaveChangesAsync();
    }

    // ========== 🆕 API-Compatible Snapshot Duplication ==========

    [HttpGet("{id}/duplicate")]
    public virtual async Task<ActionResult<TViewModel>> Duplicate(int id)
    {
        var original = await _context.Set<TModel>().FindAsync(id);
        if (original == null) return NotFound();

        var newEntity = _mapper.Map<TViewModel>(original);
        newEntity.Id = 0;

        var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
        snapshotProp?.SetValue(newEntity, null);

        return Ok(newEntity);
    }

    [HttpGet("grouped-by-name")]
    public virtual async Task<ActionResult<Dictionary<string, List<TViewModel>>>> GroupedByName()
    {
        var list = await GetQueryable().ToListAsync();
        var viewModels = _mapper.Map<List<TViewModel>>(list);
        var grouped = viewModels
            .Where(vm => vm.GetType().GetProperty("Name")?.GetValue(vm) != null)
            .GroupBy(vm => vm.GetType().GetProperty("Name")?.GetValue(vm)?.ToString() ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.ToList());

        return Ok(grouped);
    }

    [HttpGet("{id}/new-snapshot")]
    public virtual async Task<IActionResult> CreateNewSnapshot(int id)
    {
        var original = await _context.Set<TModel>().FindAsync(id);
        if (original == null)
            return NotFound();

        var viewModel = _mapper.Map<TViewModel>(original);

        viewModel.Id = 0; // Reset Id so the frontend treats it as a new entity

        // Nullify SnapshotId so the user picks a new one
        var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
        snapshotProp?.SetValue(viewModel, null);

        return Ok(viewModel);
    }


    [HttpGet("{id}/new-snapshot-page")]
    public virtual async Task<IActionResult> CreateNewSnapshotPage(int id)
    {
        var modelType = typeof(TModel);
        var navPropsToInclude = new[] { "Faction", "Location", "Language", "Snapshot", "Event", "Era" };

        var query = _context.Set<TModel>().AsQueryable();
        foreach (var navProp in navPropsToInclude)
        {
            if (modelType.GetProperty(navProp) != null)
                query = query.Include(navProp);
        }

        var original = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        if (original == null) return NotFound();

        var newSnapshot = _mapper.Map<TViewModel>(original);
        newSnapshot.Id = 0;

        var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
        snapshotProp?.SetValue(newSnapshot, null);

        var nameProp = typeof(TViewModel).GetProperty("Name");
        var nameValue = nameProp?.GetValue(newSnapshot)?.ToString();

        var versionsQuery = _context.Set<TModel>().AsQueryable().AsNoTracking();
        foreach (var navProp in navPropsToInclude)
        {
            if (modelType.GetProperty(navProp) != null)
                versionsQuery = versionsQuery.Include(navProp);
        }

        var existingVersions = string.IsNullOrEmpty(nameValue)
            ? new List<TModel>()
            : await versionsQuery.Where(e => EF.Property<string>(e, "Name") == nameValue).ToListAsync();

        var existingVMs = _mapper.Map<List<TViewModel>>(existingVersions);

        var result = new SnapshotEditPageViewModel<TViewModel>
        {
            NewSnapshot = newSnapshot,
            ExistingVersions = existingVMs
        };

        return Ok(result);
    }
}
