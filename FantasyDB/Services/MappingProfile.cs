using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using static FantasyDB.Models.JunctionClasses;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Language
        CreateMap<Location, LocationViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentLocationName,
                opt => opt.MapFrom(src => src.ParentLocation != null ? src.ParentLocation.Name : null))
            .ForMember(dest => dest.SnapshotName,
                opt => opt.MapFrom(src => src.Snapshot != null ? src.Snapshot.SnapshotName : null))
            .ForMember(dest => dest.LanguageIds,
                opt => opt.MapFrom(src => (src.LanguageLocations ?? new List<LanguageLocation>()).Select(ll => ll.LanguageId)))
            .ForMember(dest => dest.LanguageNames,
                opt => opt.MapFrom(src => (src.LanguageLocations ?? new List<LanguageLocation>()).Select(ll => ll.Language.Name)))
            .ForMember(dest => dest.EventIds,
                opt => opt.MapFrom(src => (src.Events ?? new List<Event>()).Select(e => e.Id)))
            .ForMember(dest => dest.EventNames,
                opt => opt.MapFrom(src => (src.Events ?? new List<Event>()).Select(e => e.Name)));

        CreateMap<LocationViewModel, Location>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ParentLocationId, opt => opt.MapFrom(src => src.ParentLocationId))
            .ForMember(dest => dest.SnapshotId, opt => opt.MapFrom(src => src.SnapshotId))
            .ForMember(dest => dest.LanguageLocations, opt => opt.Ignore()) // handled manually
            .ForMember(dest => dest.Events, opt => opt.Ignore());           // handled manually


        CreateMap<Language, LanguageViewModel>()
    .ForMember(dest => dest.LocationIds, opt => opt.MapFrom(src => src.LanguageLocations.Select(ll => ll.LocationId)))
    .ForMember(dest => dest.LocationNames, opt => opt.MapFrom(src => src.LanguageLocations.Select(ll => ll.Location.Name)));

        CreateMap<LanguageViewModel, Language>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LanguageLocations, opt => opt.Ignore()); // handled manually



        CreateMap<Event, EventViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));

        CreateMap<EventViewModel, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
            .ForMember(dest => dest.SnapshotId, opt => opt.MapFrom(src => src.SnapshotId));


        // Item
        CreateMap<Item, ItemViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        // Item
        CreateMap<ItemViewModel, Item>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id


        // Calendar
        CreateMap<Calendar, CalendarViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<CalendarViewModel, Calendar>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id

        // Character
        CreateMap<Character, CharacterViewModel>()
            .ForMember(dest => dest.FactionName, opt => opt.MapFrom(src => src.Faction.Name))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));

        CreateMap<CharacterViewModel, Character>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
            .ForMember(dest => dest.FactionId, opt => opt.MapFrom(src => src.FactionId))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
            .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
            .ForMember(dest => dest.SnapshotId, opt => opt.MapFrom(src => src.SnapshotId));


        // CharacterRelationship
        CreateMap<CharacterRelationship, CharacterRelationshipViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Character1Name, opt => opt.MapFrom(src => src.Character1.Name))
            .ForMember(dest => dest.Character2Name, opt => opt.MapFrom(src => src.Character2.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<CharacterRelationshipViewModel, CharacterRelationship>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
            .ForMember(dest => dest.Character1Id, opt => opt.MapFrom(src => src.Character1Id))
            .ForMember(dest => dest.Character2Id, opt => opt.MapFrom(src => src.Character2Id))
            .ForMember(dest => dest.SnapshotId, opt => opt.MapFrom(src => src.SnapshotId));

        // Currency
        CreateMap<Currency, CurrencyViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<CurrencyViewModel, Currency>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id

        // Era
        CreateMap<Era, EraViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<EraViewModel, Era>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
            .ForMember(dest => dest.SnapshotId, opt => opt.MapFrom(src => src.SnapshotId));


        // Faction
        CreateMap<Faction, FactionViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FounderName, opt => opt.MapFrom(src => src.Founder.Name))
            .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.Name))
            .ForMember(dest => dest.HQLocationName, opt => opt.MapFrom(src => src.HQLocation.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<FactionViewModel, Faction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
            .ForMember(dest => dest.FounderId, opt => opt.MapFrom(src => src.FounderId))
            .ForMember(dest => dest.LeaderId, opt => opt.MapFrom(src => src.LeaderId))
            .ForMember(dest => dest.HQLocationId, opt => opt.MapFrom(src => src.HQLocationId))
            .ForMember(dest => dest.SnapshotId, opt => opt.MapFrom(src => src.SnapshotId));

  
        // PriceExample
        CreateMap<PriceExample, PriceExampleViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<PriceExampleViewModel, PriceExample>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id

        // River
        CreateMap<River, RiverViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SourceLocationName, opt => opt.MapFrom(src => src.SourceLocation.Name))
            .ForMember(dest => dest.DestinationLocationName, opt => opt.MapFrom(src => src.DestinationLocation.Name));
        CreateMap<RiverViewModel, River>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
            .ForMember(dest => dest.SourceLocationId, opt => opt.MapFrom(src => src.SourceLocationId))
            .ForMember(dest => dest.DestinationLocationId, opt => opt.MapFrom(src => src.DestinationLocationId));

        // Route
        CreateMap<Route, RouteViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FromLocationName, opt => opt.MapFrom(src => src.From.Name))
            .ForMember(dest => dest.ToLocationName, opt => opt.MapFrom(src => src.To.Name));
        CreateMap<RouteViewModel, Route>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
            .ForMember(dest => dest.FromId, opt => opt.MapFrom(src => src.FromId))
            .ForMember(dest => dest.ToId, opt => opt.MapFrom(src => src.ToId));

        // Snapshot
        CreateMap<Snapshot, SnapshotViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.SnapshotName));
        CreateMap<SnapshotViewModel, Snapshot>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id



        CreateMap<PlotPoint, PlotPointViewModel>()
    .ForMember(dest => dest.CharacterIds, opt =>
        opt.MapFrom(src => src.PlotPointCharacters.Select(pc => pc.CharacterId)))
    .ForMember(dest => dest.LocationIds, opt =>
        opt.MapFrom(src => src.PlotPointLocations.Select(pl => pl.LocationId)))
    .ForMember(dest => dest.EventIds, opt =>
        opt.MapFrom(src => src.PlotPointEvents.Select(pe => pe.EventId)))
    .ForMember(dest => dest.FactionIds, opt =>
        opt.MapFrom(src => src.PlotPointFactions.Select(pf => pf.FactionId)))
    .ForMember(dest => dest.ItemIds, opt =>
        opt.MapFrom(src => src.PlotPointItems.Select(pa => pa.ItemId)))
    .ForMember(dest => dest.CharacterRelationshipIds, opt =>
        opt.MapFrom(src => src.PlotPointCharacterRelationships.Select(pr => pr.CharacterRelationshipId)))
    .ForMember(dest => dest.EraIds, opt =>
        opt.MapFrom(src => src.PlotPointEras.Select(pe => pe.EraId)))
    .ForMember(dest => dest.RiverIds, opt =>
        opt.MapFrom(src => src.PlotPointRivers.Select(pr => pr.RiverId)))
    .ForMember(dest => dest.RouteIds, opt =>
        opt.MapFrom(src => src.PlotPointRoutes.Select(pr => pr.RouteId)))
    .ForMember(dest => dest.CalendarLabel, opt => opt.Ignore()) // You assign this manually later
    .ForMember(dest => dest.CharacterNames, opt => opt.Ignore())
    .ForMember(dest => dest.LocationNames, opt => opt.Ignore())
    .ForMember(dest => dest.EventNames, opt => opt.Ignore())
    .ForMember(dest => dest.FactionNames, opt => opt.Ignore());



    }
}
