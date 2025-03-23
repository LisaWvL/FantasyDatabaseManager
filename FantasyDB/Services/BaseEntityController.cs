using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static FantasyDB.Models.JunctionClasses;

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

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<TModel>(viewModel);
            _context.Add(entity);
            await _context.SaveChangesAsync();

            await HandleJunctionsAfterCreate(entity, viewModel);

            var createdViewModel = _mapper.Map<TViewModel>(entity);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TViewModel viewModel)
        {
            if (viewModel == null || id != viewModel.Id)
                return BadRequest("Invalid request");

            var entity = await _context.Set<TModel>().FindAsync(id);
            if (entity == null)
                return NotFound();

            _mapper.Map(viewModel, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            await HandleJunctionsAfterUpdate(entity, viewModel);

            return Ok(new { message = "Entity updated" });
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

        protected void SetCurrentEntityName()
        {
            var name = typeof(TModel).Name;
            ViewData["CurrentEntity"] = name;
        }

        protected async Task LoadDropdownsForViewModel<T>()
        {
            await _dropdownService.LoadDropdownsForViewModel<T>(ViewData);
        }


        [HttpGet("{id}/new-snapshot")]
        public virtual async Task<IActionResult> CreateNewSnapshot(int id)
        {
            var original = await _context.Set<TModel>().FindAsync(id);
            if (original == null)
                return NotFound();

            var viewModel = _mapper.Map<TViewModel>(original);
            viewModel.Id = 0;

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

            var snapshotProp = typeof(TViewModel).GetProperty("SnapshotId");
            snapshotProp?.SetValue(viewModel, null);

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

        private async Task HandleJunctionsAfterCreate(TModel entity, TViewModel viewModel)
        {
            await HandleJunctions(entity, viewModel, isUpdate: false);
        }

        private async Task HandleJunctionsAfterUpdate(TModel entity, TViewModel viewModel)
        {
            await HandleJunctions(entity, viewModel, isUpdate: true);
        }

        private async Task HandleJunctions(TModel entity, TViewModel viewModel, bool isUpdate)
        {
            var entityName = typeof(TModel).Name;
            var entityId = (int)typeof(TModel).GetProperty("Id")?.GetValue(entity)!;

            if (entityName == "Location" && viewModel is LocationViewModel lvm)
            {
                if (isUpdate)
                {
                    _context.LocationLocation.RemoveRange(_context.LocationLocation.Where(l => l.LocationId == entityId));
                    _context.LocationEvent.RemoveRange(_context.LocationEvent.Where(l => l.LocationId == entityId));
                }

                foreach (var childId in lvm.ChildLocationIds ?? new List<int>())
                    _context.LocationLocation.Add(new LocationLocation { LocationId = entityId, ChildLocationId = childId });

                foreach (var eventId in lvm.EventIds ?? new List<int>())
                    _context.LocationEvent.Add(new LocationEvent { LocationId = entityId, EventId = eventId });

                await _context.SaveChangesAsync();
            }

            if (entityName == "Language" && viewModel is LanguageViewModel langVM)
            {
                if (isUpdate)
                    _context.LanguageLocation.RemoveRange(_context.LanguageLocation.Where(ll => ll.LanguageId == entityId));

                foreach (var locId in langVM.LocationIds ?? new List<int>())
                    _context.LanguageLocation.Add(new LanguageLocation { LanguageId = entityId, LocationId = locId });

                await _context.SaveChangesAsync();
            }

            if (typeof(TModel) == typeof(Character) && viewModel is CharacterViewModel c)
            {
                if (c.SnapshotId.HasValue)
                {
                    _context.SnapshotCharacter.Add(new SnapshotCharacter { CharacterId = entityId, SnapshotId = c.SnapshotId });
                    await _context.SaveChangesAsync();
                }
            }

            if (typeof(TModel) == typeof(Artifact) && viewModel is ArtifactViewModel a)
            {
                if (a.SnapshotId.HasValue)
                {
                    _context.SnapshotArtifact.Add(new SnapshotArtifact { ArtifactId = entityId, SnapshotId = a.SnapshotId });
                    await _context.SaveChangesAsync();
                }
            }

            if (typeof(TModel) == typeof(Event) && viewModel is EventViewModel ev)
            {
                if (ev.SnapshotId.HasValue)
                {
                    _context.SnapshotEvent.Add(new SnapshotEvent { EventId = entityId, SnapshotId = ev.SnapshotId });
                    await _context.SaveChangesAsync();
                }
            }

            if (typeof(TModel) == typeof(Era) && viewModel is EraViewModel era)
            {
                if (era.SnapshotId.HasValue)
                {
                    _context.SnapshotEra.Add(new SnapshotEra { EraId = entityId, SnapshotId = era.SnapshotId });
                    await _context.SaveChangesAsync();
                }
            }

            if (typeof(TModel) == typeof(Faction) && viewModel is FactionViewModel f)
            {
                if (f.SnapshotId.HasValue)
                {
                    _context.SnapshotFaction.Add(new SnapshotFaction { FactionId = entityId, SnapshotId = f.SnapshotId });
                    await _context.SaveChangesAsync();
                }
            }

            if (typeof(TModel) == typeof(Location) && viewModel is LocationViewModel loc)
            {
                if (loc.SnapshotId.HasValue)
                {
                    _context.SnapshotLocation.Add(new SnapshotLocation { LocationId = entityId, SnapshotId = loc.SnapshotId });
                    await _context.SaveChangesAsync();
                }
            }

            if (typeof(TModel) == typeof(CharacterRelationship) && viewModel is CharacterRelationshipViewModel cr)
            {
                if (cr.SnapshotId.HasValue)
                {
                    _context.SnapshotCharacterRelationship.Add(new SnapshotCharacterRelationship
                    {
                        CharacterRelationshipId = entityId,
                        SnapshotId = cr.SnapshotId
                    });
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}