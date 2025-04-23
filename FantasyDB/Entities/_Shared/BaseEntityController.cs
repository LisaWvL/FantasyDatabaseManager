using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FantasyDB.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace FantasyDB.Entities._Shared;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseEntityController<TModel, TViewModel>(AppDbContext context, IMapper mapper, IDropdownService dropdownService) : ControllerBase
    where TModel : class, new()
    where TViewModel : class, IViewModelWithId
{
    protected readonly AppDbContext _context = context;
    protected readonly IMapper _mapper = mapper;
    protected readonly IDropdownService _dropdownService = dropdownService;

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

        await HandleChapterLinks(entity, viewModel, entityId);

        foreach (var prop in typeof(TViewModel).GetProperties())
        {
            var junctionAttr = prop.GetCustomAttribute<HandlesJunctionAttribute>();
            if (junctionAttr == null) continue;

            if (prop.GetValue(viewModel) is not IEnumerable<int> selectedIds) continue;

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

    protected async Task HandleChapterLinks(TModel entity, TViewModel viewModel, int entityId)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var chapterProp = typeof(TViewModel).GetProperty("ChapterId");
        if (chapterProp == null) return;

        var chapterId = chapterProp.GetValue(viewModel) as int?;
        if (!chapterId.HasValue) return;

        await _context.SaveChangesAsync();
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

    [HttpGet("{id}/new-chapter")]
    public virtual async Task<IActionResult> CreateNewChapter(int id)
    {
        var original = await _context.Set<TModel>().FindAsync(id);
        if (original == null)
            return NotFound();

        var viewModel = _mapper.Map<TViewModel>(original);

        viewModel.Id = 0; // Reset Id so the frontend treats it as a new entity

        // Nullify ChapterId so the user picks a new one
        var chapterProp = typeof(TViewModel).GetProperty("ChapterId");
        chapterProp?.SetValue(viewModel, null);

        return Ok(viewModel);
    }


    [HttpGet("{id}/new-chapter-page")]
    public virtual async Task<IActionResult> CreateNewWritingAssistantPage(int id)
    {
        var modelType = typeof(TModel);
        var navPropsToInclude = new[] { "Faction", "Location", "Language", "Chapter", "Event", "Era" };

        var query = _context.Set<TModel>().AsQueryable();
        foreach (var navProp in navPropsToInclude)
        {
            if (modelType.GetProperty(navProp) != null)
                query = query.Include(navProp);
        }

        var original = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        if (original == null) return NotFound();

        var newChapter = _mapper.Map<TViewModel>(original);
        newChapter.Id = 0;

        var chapterProp = typeof(TViewModel).GetProperty("ChapterId");
        chapterProp?.SetValue(newChapter, null);

        var nameProp = typeof(TViewModel).GetProperty("Name");
        var nameValue = nameProp?.GetValue(newChapter)?.ToString();

        var versionsQuery = _context.Set<TModel>().AsQueryable().AsNoTracking();
        foreach (var navProp in navPropsToInclude)
        {
            if (modelType.GetProperty(navProp) != null)
                versionsQuery = versionsQuery.Include(navProp);
        }

        var existingVersions = string.IsNullOrEmpty(nameValue)
            ? []
            : await versionsQuery.Where(e => EF.Property<string>(e, "Name") == nameValue).ToListAsync();

        var existingVMs = _mapper.Map<List<TViewModel>>(existingVersions);

        var result = new ChapterEditPageViewModel<TViewModel>
        {
            NewChapter = newChapter,
            ExistingVersions = existingVMs
        };

        return Ok(result);
    }
}
