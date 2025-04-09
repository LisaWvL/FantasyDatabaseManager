using FantasyDB.Models;
using FantasyDB.ViewModels;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.Operations;
using FantasyDB.Interfaces;

public class EntityRegistryService : IEntityRegistryService
{
    private static readonly Dictionary<string, (Type, Type)> _map = new()
    {
        { "Character", (typeof(Character), typeof(CharacterViewModel)) },
        { "Faction", (typeof(Faction), typeof(FactionViewModel)) },
        { "Item", (typeof(Item), typeof(ItemViewModel)) },
        { "CharacterRelationship", (typeof(CharacterRelationship), typeof(CharacterRelationshipViewModel)) },
        { "PlotPoint", (typeof(PlotPoint), typeof(PlotPointViewModel)) },
        { "ConversationTurn", (typeof(ConversationTurn), typeof(ConversationTurnViewModel)) },
        { "PlotPointRiver", (typeof(FantasyDB.Models.JunctionClasses.PlotPointRiver), typeof(FantasyDB.ViewModels.JunctionClassesViewModels.PlotPointRiverViewModel)) },
        { "PlotPointRoute", (typeof(FantasyDB.Models.JunctionClasses.PlotPointRoute), typeof(FantasyDB.ViewModels.JunctionClassesViewModels.PlotPointRouteViewModel)) },
        {"LanguageLocation", (typeof(FantasyDB.Models.JunctionClasses.LanguageLocation), typeof(FantasyDB.ViewModels.JunctionClassesViewModels.LanguageLocationViewModel)) },
        { "Event", (typeof(Event), typeof(EventViewModel)) },
        { "Era", (typeof(Era), typeof(EraViewModel)) },
        { "Language", (typeof(Language), typeof(LanguageViewModel)) },
        { "Location", (typeof(Location), typeof(LocationViewModel)) },
        { "PriceExample", (typeof(PriceExample), typeof(PriceExample)) },
        { "River", (typeof(River), typeof(RiverViewModel)) },
        { "Route", (typeof(FantasyDB.Models.Route), typeof(RouteViewModel)) },
        { "Chapter", (typeof(Chapter), typeof(ChapterViewModel)) },
        { "Calendar", (typeof(Calendar), typeof(CalendarViewModel)) },
        { "Currency", (typeof(Currency), typeof(CurrencyViewModel)) }
    };

    public Dictionary<string, (Type, Type)> GetEntityMap() => _map;
}
