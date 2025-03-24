using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static FantasyDB.Models.JunctionClasses;
using System.Text.Json;
using System.Reflection;
using FantasyDB.Attributes; // Make sure you define the [HandlesJunction] attribute here
using System;


namespace FantasyDB.Services
{
    [Route("api/[controller]")]
    public abstract class BaseEntityController<TModel, TViewModel> : Controller
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
        public virtual async Task<IActionResult> Index()
        {
            var entities = await GetQueryable().AsNoTracking().ToListAsync();
            var viewModels = _mapper.Map<List<TViewModel>>(entities);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);
            if (entity == null)
                return NotFound();

            var viewModel = _mapper.Map<TViewModel>(entity);
            return Ok(viewModel);
        }

        [HttpPost("create")]
        public virtual async Task<IActionResult> Create([FromBody] TViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<TModel>(viewModel);
            _context.Add(entity);
            await _context.SaveChangesAsync();

            await HandleJunctions(entity, viewModel, isUpdate: false);

            var createdViewModel = _mapper.Map<TViewModel>(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _context.Set<TModel>().FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(viewModel, entity);
            await _context.SaveChangesAsync();

            await HandleJunctions(entity, viewModel, isUpdate: true);
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Entity deleted" });
        }

        protected async Task LoadDropdownsForViewModel<T>()
        {
            await _dropdownService.LoadDropdownsForViewModel<T>(ViewData);
        }

        protected void SetCurrentEntityName()
        {
            ViewData["CurrentEntity"] = typeof(TModel).Name;
        }

        // ------------------------------------------------------
        // 🔁 Unified Generic Junction Handler with Attribute Support
        // ------------------------------------------------------
        private async Task HandleJunctions(TModel entity, TViewModel viewModel, bool isUpdate)
        {
            var entityId = (int)(typeof(TModel).GetProperty("Id")?.GetValue(entity) ?? 0);

            // 1. Handle snapshot relationships (manually mapped ones)
            await HandleSnapshotLinks(entity, viewModel, entityId);

            // 2. Handle generic [HandlesJunction] mappings
            foreach (var prop in typeof(TViewModel).GetProperties())
            {
                var junctionAttr = prop.GetCustomAttribute<HandlesJunctionAttribute>();
                if (junctionAttr == null) continue;

                var selectedIds = prop.GetValue(viewModel) as IEnumerable<int>;
                if (selectedIds == null) continue;

                // Get the junction entity type
                var junctionType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == junctionAttr.JunctionEntity);

                if (junctionType == null)
                {
                    Console.WriteLine($"[JUNCTION] Could not find type: {junctionAttr.JunctionEntity}");
                    continue;
                }

                // Get the DbSet for the junction type
                var junctionSet = _context.GetType()
                    .GetMethod("Set", Type.EmptyTypes)!
                    .MakeGenericMethod(junctionType)
                    .Invoke(_context, null);

                if (junctionSet is not IQueryable junctionQuery) continue;

                // Materialize to list so we can filter with runtime reflection
                var allJunctions = junctionQuery.Cast<object>().ToList();

                var toRemove = allJunctions
                    .Where(j =>
                    {
                        var thisKeyValue = j.GetType().GetProperty(junctionAttr.ThisKey)?.GetValue(j);
                        return thisKeyValue != null && (int)thisKeyValue == entityId;
                    })
                    .ToList();

                foreach (var j in toRemove)
                {
                    _context.Remove(j);
                }


                // Add new junction entries
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

        private async Task HandleSnapshotLinks(TModel entity, TViewModel viewModel, int entityId)
        {
            var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
            if (snapshotProp == null) return;

            var snapshotId = snapshotProp.GetValue(viewModel) as int?;
            if (!snapshotId.HasValue) return;

            if (typeof(TModel) == typeof(Character))
                _context.SnapshotCharacter.Add(new SnapshotCharacter { CharacterId = entityId, SnapshotId = snapshotId.Value });

            else if (typeof(TModel) == typeof(Artifact))
                _context.SnapshotArtifact.Add(new SnapshotArtifact { ArtifactId = entityId, SnapshotId = snapshotId.Value });

            else if (typeof(TModel) == typeof(Location))
                _context.SnapshotLocation.Add(new SnapshotLocation { LocationId = entityId, SnapshotId = snapshotId.Value });

            else if (typeof(TModel) == typeof(Faction))
                _context.SnapshotFaction.Add(new SnapshotFaction { FactionId = entityId, SnapshotId = snapshotId.Value });

            else if (typeof(TModel) == typeof(Era))
                _context.SnapshotEra.Add(new SnapshotEra { EraId = entityId, SnapshotId = snapshotId.Value });

            else if (typeof(TModel) == typeof(Event))
                _context.SnapshotEvent.Add(new SnapshotEvent { EventId = entityId, SnapshotId = snapshotId.Value });

            else if (typeof(TModel) == typeof(CharacterRelationship))
                _context.SnapshotCharacterRelationship.Add(new SnapshotCharacterRelationship { CharacterRelationshipId = entityId, SnapshotId = snapshotId.Value });

            await _context.SaveChangesAsync();
        }

        [HttpGet("{id}/new-snapshot")]
        public virtual async Task<IActionResult> CreateNewSnapshot(int id)
        {
            var original = await _context.Set<TModel>().FindAsync(id);
            if (original == null)
                return NotFound();

            var viewModel = _mapper.Map<TViewModel>(original);
            viewModel.Id = 0; // Reset Id for the new entity

            // Reset SnapshotId
            var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
            snapshotProp?.SetValue(viewModel, null);

            await LoadDropdownsForViewModel<TViewModel>();
            ViewData["CurrentEntity"] = typeof(TModel).Name;

            return View("_EntityTableRow", new List<TViewModel> { viewModel });
        }

        [HttpGet("{id}/new-snapshot-page")]
        public virtual async Task<IActionResult> CreateNewSnapshotPage(int id)
        {
            var modelType = typeof(TModel);
            var navPropsToInclude = new[] { "Faction", "Location", "Language", "Snapshot", "Event", "Era" };

            var originalQuery = _context.Set<TModel>().AsQueryable();
            foreach (var navProp in navPropsToInclude)
            {
                if (modelType.GetProperty(navProp) != null)
                    originalQuery = originalQuery.Include(navProp);
            }

            var original = await originalQuery.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            if (original == null) return NotFound();

            var viewModel = _mapper.Map<TViewModel>(original);
            viewModel.Id = 0;

            // Reset SnapshotId
            var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
            snapshotProp?.SetValue(viewModel, null);

            // Try to get the "Name" value to group other versions
            var nameProp = typeof(TViewModel).GetProperty("Name");
            var nameValue = nameProp?.GetValue(viewModel)?.ToString();

            var otherVersionsQuery = _context.Set<TModel>().AsQueryable().AsNoTracking();
            foreach (var navProp in navPropsToInclude)
            {
                if (modelType.GetProperty(navProp) != null)
                    otherVersionsQuery = otherVersionsQuery.Include(navProp);
            }

            var otherVersions = string.IsNullOrEmpty(nameValue)
                ? new List<TModel>()
                : await otherVersionsQuery.Where(e => EF.Property<string>(e, "Name") == nameValue).ToListAsync();

            var versionVMs = _mapper.Map<List<TViewModel>>(otherVersions);

            await LoadDropdownsForViewModel<TViewModel>();
            ViewData["CurrentEntity"] = typeof(TModel).Name;

            var pageModel = new SnapshotEditPageViewModel<TViewModel>
            {
                NewSnapshot = viewModel,
                ExistingVersions = versionVMs
            };

            return View("~/Views/Shared/CreateNewSnapshot.cshtml", pageModel);
        }


    }
}
