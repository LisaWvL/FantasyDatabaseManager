using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FantasyDB.Models;
using FantasyDB.ViewModels;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Language
            CreateMap<Location, LocationViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ParentLocationName,
                    opt => opt.MapFrom(src => src.ParentLocation != null ? src.ParentLocation.Name : null))
                .ForMember(dest => dest.ChapterNumber,
                    opt => opt.MapFrom(src => src.Chapter != null ? src.Chapter.ChapterNumber : null))
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
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
                .ForMember(dest => dest.LanguageLocations, opt => opt.Ignore()) // handled manually
                .ForMember(dest => dest.Events, opt => opt.Ignore());           // handled manually


            CreateMap<Language, LanguageViewModel>()
        .ForMember(dest => dest.LocationIds, opt => opt.MapFrom(src => src.LanguageLocations.Select(ll => ll.LocationId)))
        .ForMember(dest => dest.LocationNames, opt => opt.MapFrom(src => src.LanguageLocations.Select(ll => ll.Location.Name)));

            CreateMap<LanguageViewModel, Language>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LanguageLocations, opt => opt.Ignore()); // handled manually



#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.ChapterNumber));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            CreateMap<EventViewModel, Event>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));


            // Item
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Item, ItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.ChapterNumber));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            // Item
            CreateMap<ItemViewModel, Item>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id


            // Calendar
            CreateMap<Calendar, CalendarViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<CalendarViewModel, Calendar>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id

            // Character
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Character, CharacterViewModel>()
                .ForMember(dest => dest.FactionName, opt => opt.MapFrom(src => src.Faction.Name))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Name))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.ChapterNumber));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            CreateMap<CharacterViewModel, Character>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
                .ForMember(dest => dest.FactionId, opt => opt.MapFrom(src => src.FactionId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));


            // CharacterRelationship
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<CharacterRelationship, CharacterRelationshipViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Character1Name, opt => opt.MapFrom(src => src.Character1.Name))
                .ForMember(dest => dest.Character2Name, opt => opt.MapFrom(src => src.Character2.Name))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.ChapterNumber));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CreateMap<CharacterRelationshipViewModel, CharacterRelationship>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
                .ForMember(dest => dest.Character1Id, opt => opt.MapFrom(src => src.Character1Id))
                .ForMember(dest => dest.Character2Id, opt => opt.MapFrom(src => src.Character2Id))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));

            // Currency
            CreateMap<Currency, CurrencyViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<CurrencyViewModel, Currency>()
                            .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id

            // Era
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Era, EraViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.ChapterNumber));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CreateMap<EraViewModel, Era>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));


            // Faction
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Faction, FactionViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FounderName, opt => opt.MapFrom(src => src.Founder.Name))
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.Name))
                .ForMember(dest => dest.HQLocationName, opt => opt.MapFrom(src => src.HQLocation.Name))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter.ChapterNumber));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CreateMap<FactionViewModel, Faction>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
                .ForMember(dest => dest.FounderId, opt => opt.MapFrom(src => src.FounderId))
                .ForMember(dest => dest.LeaderId, opt => opt.MapFrom(src => src.LeaderId))
                .ForMember(dest => dest.HQLocationId, opt => opt.MapFrom(src => src.HQLocationId))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));


            // PriceExample
            CreateMap<PriceExample, PriceExampleViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<PriceExampleViewModel, PriceExample>()
                            .ForMember(dest => dest.Id, opt => opt.Ignore()); // 👈 prevent AutoMapper from setting Id

            // River
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<River, RiverViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SourceLocationName, opt => opt.MapFrom(src => src.SourceLocation.Name))
                .ForMember(dest => dest.DestinationLocationName, opt => opt.MapFrom(src => src.DestinationLocation.Name));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CreateMap<RiverViewModel, River>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
                .ForMember(dest => dest.SourceLocationId, opt => opt.MapFrom(src => src.SourceLocationId))
                .ForMember(dest => dest.DestinationLocationId, opt => opt.MapFrom(src => src.DestinationLocationId));

            // Route
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Route, RouteViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FromLocationName, opt => opt.MapFrom(src => src.From.Name))
                .ForMember(dest => dest.ToLocationName, opt => opt.MapFrom(src => src.To.Name));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CreateMap<RouteViewModel, Route>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // <-- 👈 Important!
                .ForMember(dest => dest.FromId, opt => opt.MapFrom(src => src.FromId))
                .ForMember(dest => dest.ToId, opt => opt.MapFrom(src => src.ToId));


            // BOOK → BOOKVIEWMODEL
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BookNumber, opt => opt.MapFrom(src => src.BookNumber))
                .ForMember(dest => dest.SeriesTitle, opt => opt.MapFrom(src => src.SeriesTitle))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookTitle))
                .ForMember(dest => dest.BookWordCount, opt => opt.MapFrom(src => src.BookWordCount))
                .ForMember(dest => dest.BookSummary, opt => opt.MapFrom(src => src.BookSummary))
                .ForMember(dest => dest.BookToDo, opt => opt.MapFrom(src => src.BookToDo))
                .ForMember(dest => dest.Acts, opt => opt.MapFrom(src => src.Acts));

            // BOOKVIEWMODEL → BOOK
            CreateMap<BookViewModel, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookNumber, opt => opt.MapFrom(src => src.BookNumber))
                .ForMember(dest => dest.SeriesTitle, opt => opt.MapFrom(src => src.SeriesTitle))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookTitle))
                .ForMember(dest => dest.BookWordCount, opt => opt.MapFrom(src => src.BookWordCount))
                .ForMember(dest => dest.BookSummary, opt => opt.MapFrom(src => src.BookSummary))
                .ForMember(dest => dest.BookToDo, opt => opt.MapFrom(src => src.BookToDo))
                .ForMember(dest => dest.Acts, opt => opt.Ignore()); // 💡 Safer to update separately


            // ACT → ACTVIEWMODEL
            CreateMap<Act, ActViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ActTitle, opt => opt.MapFrom(src => src.ActTitle))
                .ForMember(dest => dest.ActNumber, opt => opt.MapFrom(src => src.ActNumber))
                .ForMember(dest => dest.ActSummary, opt => opt.MapFrom(src => src.ActSummary))
                .ForMember(dest => dest.ActToDo, opt => opt.MapFrom(src => src.ActToDo))
                .ForMember(dest => dest.ActWordCount, opt => opt.MapFrom(src => src.ActWordCount))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.BookTitle))
                .ForMember(dest => dest.BookNumber, opt => opt.MapFrom(src => src.Book.BookNumber))
                .ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters));

            // ACTVIEWMODEL → ACT
            CreateMap<ActViewModel, Act>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ActTitle, opt => opt.MapFrom(src => src.ActTitle))
                .ForMember(dest => dest.ActNumber, opt => opt.MapFrom(src => src.ActNumber))
                .ForMember(dest => dest.ActSummary, opt => opt.MapFrom(src => src.ActSummary))
                .ForMember(dest => dest.ActToDo, opt => opt.MapFrom(src => src.ActToDo))
                .ForMember(dest => dest.ActWordCount, opt => opt.MapFrom(src => src.ActWordCount))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Book, opt => opt.Ignore())       // Prevent tracking overwrite
                .ForMember(dest => dest.Chapters, opt => opt.Ignore());  // Optional: update separately



            // CHAPTER → CHAPTERVIEWMODEL
            CreateMap<Chapter, ChapterViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.ChapterNumber))
                .ForMember(dest => dest.ChapterTitle, opt => opt.MapFrom(src => src.ChapterTitle))
                .ForMember(dest => dest.ChapterText, opt => opt.MapFrom(src => src.ChapterText))
                .ForMember(dest => dest.ChapterSummary, opt => opt.MapFrom(src => src.ChapterSummary))
                .ForMember(dest => dest.WordCount, opt => opt.MapFrom(src => src.WordCount))
                .ForMember(dest => dest.ToDo, opt => opt.MapFrom(src => src.ToDo))
                .ForMember(dest => dest.ActId, opt => opt.MapFrom(src => src.ActId))
                .ForMember(dest => dest.ActTitle, opt => opt.MapFrom(src => src.Act != null ? src.Act.ActTitle : string.Empty))
                .ForMember(dest => dest.ActNumber, opt => opt.MapFrom(src => src.Act != null ? src.Act.ActNumber : 0))
                .ForMember(dest => dest.POVCharacterId, opt => opt.MapFrom(src => src.POVCharacterId))
                .ForMember(dest => dest.POVCharacterName, opt => opt.MapFrom(src => src.POVCharacter != null ? src.POVCharacter.Name : string.Empty))
                .ForMember(dest => dest.Scenes, opt => opt.MapFrom(src => src.Scenes));

            // CHAPTERVIEWMODEL → CHAPTER
            CreateMap<ChapterViewModel, Chapter>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.ChapterNumber))
                .ForMember(dest => dest.ChapterTitle, opt => opt.MapFrom(src => src.ChapterTitle))
                .ForMember(dest => dest.ChapterText, opt => opt.MapFrom(src => src.ChapterText))
                .ForMember(dest => dest.ChapterSummary, opt => opt.MapFrom(src => src.ChapterSummary))
                .ForMember(dest => dest.WordCount, opt => opt.MapFrom(src => src.WordCount))
                .ForMember(dest => dest.ToDo, opt => opt.MapFrom(src => src.ToDo))
                .ForMember(dest => dest.ActId, opt => opt.MapFrom(src => src.ActId))
                .ForMember(dest => dest.POVCharacterId, opt => opt.MapFrom(src => src.POVCharacterId))
                .ForMember(dest => dest.Scenes, opt => opt.Ignore()); // Optional: manage scenes via separate API


            // Scene
            // SCENE → SCENEVIEWMODEL
            CreateMap<Scene, SceneViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SceneNumber, opt => opt.MapFrom(src => src.SceneNumber))
                .ForMember(dest => dest.SceneTitle, opt => opt.MapFrom(src => src.SceneTitle))
                .ForMember(dest => dest.SceneText, opt => opt.MapFrom(src => src.SceneText))
                .ForMember(dest => dest.SceneWordCount, opt => opt.MapFrom(src => src.SceneWordCount))
                .ForMember(dest => dest.SceneSummary, opt => opt.MapFrom(src => src.SceneSummary))
                .ForMember(dest => dest.SceneToDo, opt => opt.MapFrom(src => src.SceneToDo))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
                .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.Chapter != null ? src.Chapter.ChapterNumber : 0))
                .ForMember(dest => dest.ChapterTitle, opt => opt.MapFrom(src => src.Chapter != null ? src.Chapter.ChapterTitle : string.Empty));

            // SCENEVIEWMODEL → SCENE
            CreateMap<SceneViewModel, Scene>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SceneNumber, opt => opt.MapFrom(src => src.SceneNumber))
                .ForMember(dest => dest.SceneTitle, opt => opt.MapFrom(src => src.SceneTitle))
                .ForMember(dest => dest.SceneText, opt => opt.MapFrom(src => src.SceneText))
                .ForMember(dest => dest.SceneWordCount, opt => opt.MapFrom(src => src.SceneWordCount))
                .ForMember(dest => dest.SceneSummary, opt => opt.MapFrom(src => src.SceneSummary))
                .ForMember(dest => dest.SceneToDo, opt => opt.MapFrom(src => src.SceneToDo))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
                .ForMember(dest => dest.Chapter, opt => opt.Ignore()); // Prevent EF from tracking conflicts




            CreateMap<PlotPoint, PlotPointViewModel>()
                .ForMember(dest => dest.RiverIds,
                    opt => opt.MapFrom(src => src.PlotPointRivers.Select(pr => pr.RiverId)))
                .ForMember(dest => dest.RouteIds,
                    opt => opt.MapFrom(src => src.PlotPointRoutes.Select(pr => pr.RouteId)))

                // ✅ Direct mapping of scalar foreign key IDs
                .ForMember(dest => dest.StartDateId, opt => opt.MapFrom(src => src.startDateId))
                .ForMember(dest => dest.EndDateId, opt => opt.MapFrom(src => src.endDateId))
                .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))

                // ✅ You construct these manually later
                .ForMember(dest => dest.StartDateName, opt => opt.Ignore())
                .ForMember(dest => dest.EndDateName, opt => opt.Ignore())

                .ForMember(dest => dest.ChapterNumber, opt => opt.Ignore());


            CreateMap<ConversationTurn, ConversationTurnViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Prompt, opt => opt.MapFrom(src => src.Prompt))
                .ForMember(dest => dest.Response, opt => opt.MapFrom(src => src.Response))
                .ForMember(dest => dest.DanMode, opt => opt.MapFrom(src => src.DanMode))
                .ForMember(dest => dest.IsSummary, opt => opt.MapFrom(src => src.IsSummary))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.SummaryLevel, opt => opt.MapFrom(src => src.SummaryLevel))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.TokenCount, opt => opt.MapFrom(src => src.TokenCount))
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore())
                .ForMember(dest => dest.PlotPointId, opt => opt.MapFrom(src => src.PlotPointId));

            CreateMap<ConversationTurnViewModel, ConversationTurn>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Prompt, opt => opt.MapFrom(src => src.Prompt))
                .ForMember(dest => dest.Response, opt => opt.MapFrom(src => src.Response))
                .ForMember(dest => dest.DanMode, opt => opt.MapFrom(src => src.DanMode))
                .ForMember(dest => dest.IsSummary, opt => opt.MapFrom(src => src.IsSummary))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.SummaryLevel, opt => opt.MapFrom(src => src.SummaryLevel))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.TokenCount, opt => opt.MapFrom(src => src.TokenCount))
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore())
                .ForMember(dest => dest.PlotPointId, opt => opt.MapFrom(src => src.PlotPointId))
                .ForMember(dest => dest.PlotPoint, opt => opt.Ignore()); // Navigation property



        }
    }
}