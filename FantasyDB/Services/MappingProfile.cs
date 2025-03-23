using AutoMapper;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Artifact
        CreateMap<Artifact, ArtifactViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<ArtifactViewModel, Artifact>();

        // Calendar
        CreateMap<Calendar, CalendarViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<CalendarViewModel, Calendar>();

        // Character
        CreateMap<Character, CharacterViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FactionName, opt => opt.MapFrom(src => src.Faction.Name))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Type))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<CharacterViewModel, Character>();

        // CharacterRelationship
        CreateMap<CharacterRelationship, CharacterRelationshipViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Character1Name, opt => opt.MapFrom(src => src.Character1.Name))
            .ForMember(dest => dest.Character2Name, opt => opt.MapFrom(src => src.Character2.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<CharacterRelationshipViewModel, CharacterRelationship>();

        // Currency
        CreateMap<Currency, CurrencyViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<CurrencyViewModel, Currency>();

        // Era
        CreateMap<Era, EraViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<EraViewModel, Era>();

        // Event
        CreateMap<Event, EventViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<EventViewModel, Event>();

        // Faction
        CreateMap<Faction, FactionViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FounderName, opt => opt.MapFrom(src => src.Founder.Name))
            .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.Name))
            .ForMember(dest => dest.HQLocationName, opt => opt.MapFrom(src => src.HQLocation.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<FactionViewModel, Faction>();

        // Language
        CreateMap<Language, LanguageViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<LanguageViewModel, Language>();

        // Location
        CreateMap<Location, LocationViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentLocationName, opt => opt.MapFrom(src => src.ParentLocation.Name))
            .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Name))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.Snapshot.SnapshotName));
        CreateMap<LocationViewModel, Location>();

        // PriceExample
        CreateMap<PriceExample, PriceExampleViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<PriceExampleViewModel, PriceExample>();

        // River
        CreateMap<River, RiverViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SourceLocationName, opt => opt.MapFrom(src => src.SourceLocation.Name))
            .ForMember(dest => dest.DestinationLocationName, opt => opt.MapFrom(src => src.DestinationLocation.Name));
        CreateMap<RiverViewModel, River>();

        // Route
        CreateMap<Route, RouteViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FromLocationName, opt => opt.MapFrom(src => src.From.Name))
            .ForMember(dest => dest.ToLocationName, opt => opt.MapFrom(src => src.To.Name));
        CreateMap<RouteViewModel, Route>();

        // Snapshot
        CreateMap<Snapshot, SnapshotViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SnapshotName, opt => opt.MapFrom(src => src.SnapshotName));
        CreateMap<SnapshotViewModel, Snapshot>();

    }
}