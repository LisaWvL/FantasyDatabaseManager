using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using FantasyDB.Models;
using FantasyDB.Services;
using System.Text.Json;
using System.Reflection;
using FantasyDB.ViewModels;

namespace FantasyDBStartup.Controllers;

[Route("{entity}")]
[Route("{entity}/{action=Index}/{id?}")]
public class GenericEntityController : Controller
{
    private readonly ILogger<GenericEntityController> _logger;

    public GenericEntityController(AppDbContext context, IDropdownService dropdowns, ILogger<GenericEntityController> logger)
    {
        _context = context;
        _dropdowns = dropdowns;
        _logger = logger;
    }

    private readonly AppDbContext _context;
    private readonly IDropdownService _dropdowns;

    private static readonly Dictionary<string, (Type ModelType, Type ViewModelType)> _modelMap = new()
{
    { "Character", (typeof(Character), typeof(CharacterViewModel)) },
    { "Faction", (typeof(Faction), typeof(FactionViewModel)) },
    { "Artifact", (typeof(Artifact), typeof(ArtifactViewModel)) },
    { "CharacterRelationship", (typeof(CharacterRelationship), typeof(CharacterRelationshipViewModel)) },
    { "Event", (typeof(Event), typeof(EventViewModel)) },
    { "Era", (typeof(Era), typeof(EraViewModel)) },
    { "Language", (typeof(Language), typeof(LanguageViewModel)) },
    { "Location", (typeof(Location), typeof(LocationViewModel)) },
    { "PriceExample", (typeof(PriceExample), typeof(PriceExample)) },
    { "River", (typeof(River), typeof(RiverViewModel)) },
    { "Route", (typeof(FantasyDB.Models.Route), typeof(RouteViewModel)) },
    { "Snapshot", (typeof(Snapshot), typeof(SnapshotViewModel)) },
    { "Calendar", (typeof(Calendar), typeof(CalendarViewModel)) },
    { "Currency", (typeof(Currency), typeof(CurrencyViewModel)) }
};

    
    public async Task<IActionResult> Index(string entity)
    {

        if (!_modelMap.TryGetValue(entity, out var mapping))
            return NotFound($"Unknown entity type: {entity}");

        var modelType = mapping.ModelType;
        var viewModelType = mapping.ViewModelType;

        var dbSet = _context.GetType().GetProperty(entity)?.GetValue(_context);
        var list = dbSet as IQueryable<object>;
        if (list == null)
            return BadRequest("Could not resolve DbSet");

        var modelList = await list.AsNoTracking().ToListAsync();

        // ✅ Convert database models to ViewModels before returning them to the view
        var viewModelList = modelList.Select(model => ConvertToViewModel(model, modelType, viewModelType)).ToList();
        _logger.LogInformation("[Index] Requested entity: {Entity}", entity);
        _logger.LogInformation("[Index] Model type: {ModelType}, ViewModel type: {ViewModelType}", modelType.Name, viewModelType.Name);
        _logger.LogInformation("[Index] Loaded {Count} items from database", modelList.Count);

        await _dropdowns.LoadDropdowns(ViewData);
        ViewData["CurrentEntity"] = entity;

        return View("_EntityTable", viewModelList);
    }

    private object ConvertToViewModel(object model, Type modelType, Type viewModelType)
    {
        var viewModel = Activator.CreateInstance(viewModelType);
        if (viewModel == null) return model;

        foreach (var viewModelProp in viewModelType.GetProperties().Where(p => p.CanWrite))
        {
            var modelProp = modelType.GetProperty(viewModelProp.Name);
            if (modelProp != null)
            {
                viewModelProp.SetValue(viewModel, modelProp.GetValue(model));
            }
            _logger.LogDebug("[ConvertToViewModel] Mapping model to ViewModel: {ModelType} -> {ViewModelType}", modelType.Name, viewModelType.Name);
            _logger.LogDebug("[ConvertToViewModel] Found matching property: {Prop}", viewModelProp.Name);

        }

        // Handle readable name conversions based on conventions like FactionName <-> Faction.Id
        foreach (var viewModelProp in viewModelType.GetProperties().Where(p => p.Name.EndsWith("Name") && p.CanWrite))
        {

            var prefix = viewModelProp.Name.Replace("Name", "");
            var idProp = modelType.GetProperty(prefix + "Id");
            if (idProp == null) continue;

            var idValue = idProp.GetValue(model);
            if (idValue == null) continue;

            string? name = null;
            _logger.LogDebug("[ConvertToViewModel] Set {Prefix}Name = {Name}", prefix, name);

            switch (prefix)
            {

                case "Faction":
                    name = _context.Faction.FirstOrDefault(f => f.Id == (int)idValue)?.Name;
                    break;
                case "Location":
                    name = _context.Location.FirstOrDefault(l => l.Id == (int)idValue)?.Name;
                    break;
                case "Language":
                    name = _context.Language.FirstOrDefault(l => l.Id == (int)idValue)?.Type;
                    break;
                case "Snapshot":
                    name = _context.Snapshot.FirstOrDefault(s => s.Id == (int)idValue)?.SnapshotName;
                    break;
                case "Character1":
                    name = _context.Character.FirstOrDefault(c => c.Id == (int)idValue)?.Name;
                    break;
                case "Character2":
                    name = _context.Character.FirstOrDefault(c => c.Id == (int)idValue)?.Name;
                    break;
                case "Event":
                    name = _context.Event.FirstOrDefault(e => e.Id == (int)idValue)?.Name;
                    break;
                case "Era":
                    name = _context.Era.FirstOrDefault(e => e.Id == (int)idValue)?.Name;
                    break;
                case "Owner":
                    name = _context.Character.FirstOrDefault(c => c.Id == (int)idValue)?.Name;
                    break;
                case "From":
                    name = _context.Location.FirstOrDefault(l => l.Id == (int)idValue)?.Name;
                    break;
                case "To":
                    name = _context.Location.FirstOrDefault(l => l.Id == (int)idValue)?.Name;
                    break;
                case "ParentLocation":
                case "ChildLocation":
                case "SourceLocation":
                case "DestinationLocation":
                    name = _context.Location.FirstOrDefault(l => l.Id == (int)idValue)?.Name;
                    break;

            }

            if (!string.IsNullOrEmpty(name))
            {
                viewModelProp.SetValue(viewModel, name);
            }
        }

        return viewModel;
    }





    [HttpPost]
    public async Task<IActionResult> Edit(string entity, int id)
    {
        if (!_modelMap.TryGetValue(entity, out var mapping))
            return NotFound($"Unknown entity: {entity}");

        var modelType = mapping.ModelType; // ✅ Extract the actual model type

        var dbSet = _context.GetType().GetProperty(entity)?.GetValue(_context);
        if (dbSet is not IQueryable)
            return BadRequest("Could not access DbSet");

        var existing = await ((dynamic)dbSet).FindAsync(id);
        if (existing == null)
            return NotFound();

        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();
        if (string.IsNullOrWhiteSpace(json))
            return BadRequest("No data");

        var updates = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        if (updates == null)
            return BadRequest("Invalid JSON");

        foreach (var prop in modelType.GetProperties().Where(p => p.CanWrite && p.Name != "Id"))
        {
            if (updates.TryGetValue(prop.Name, out var value))
            {
                _logger.LogInformation("[Edit] Updating entity: {Entity}, Id: {Id}", entity, id);
                _logger.LogDebug("[Edit] Raw request body: {Json}", json);

                try
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    _logger.LogDebug("[Edit] Applying update to property: {Property}, Value: {Value}", prop.Name, value);

                    object? parsed = Convert.ChangeType(
                        value,
                        Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType
                    );

                    prop.SetValue(existing, parsed);
                }
                catch
                {
                    // Ignore parsing errors
                }
            }
        }

        _context.Update(existing);
       // _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Edit] SaveChanges called for {Entity} with Id: {Id}", entity, id);

        return Ok(new { message = $"{entity} updated successfully" });
    }


    [HttpPost]
    public async Task<IActionResult> Create(string entity)
    {
        if (!_modelMap.TryGetValue(entity, out var mapping))
            return NotFound($"Unknown entity: {entity}");

        var modelType = mapping.ModelType; // ✅ Extract the correct model type

        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();
        var updates = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        if (updates == null)
            return BadRequest("Invalid JSON");

        var instance = Activator.CreateInstance(modelType);
        if (instance == null)
            return BadRequest("Could not create instance");

        foreach (var prop in modelType.GetProperties().Where(p => p.CanWrite))
        {
            if (updates.TryGetValue(prop.Name, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                _logger.LogInformation("[Create] Creating new entity: {Entity}", entity);
                _logger.LogDebug("[Create] Raw request body: {Json}", json);
                _logger.LogDebug("[Create] Setting property {Property} = {Value}", prop.Name, value);

                try
                {
                    var parsed = Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    prop.SetValue(instance, parsed);
                    _logger.LogInformation("[Create] Creating new entity: {Entity}", entity);
                    _logger.LogDebug("[Create] Raw request body: {Json}", json);
                    _logger.LogDebug("[Create] Setting property {Property} = {Value}", prop.Name, value);

                }
                catch
                {
                    // Ignore errors
                }
            }
        }

        _context.Add(instance);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Create] Entity {Entity} created and saved successfully", entity);

        ViewData["CurrentEntity"] = entity;

        return Ok(new { message = $"{entity} created successfully" });
    }


}
