using System.Collections.Generic;
using System;

namespace FantasyDB.Entities._Shared;

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
        { "PlotPointRiver", (typeof(PlotPointRiver), typeof(JunctionClassesViewModels.PlotPointRiverViewModel)) },
        { "PlotPointRoute", (typeof(PlotPointRoute), typeof(JunctionClassesViewModels.PlotPointRouteViewModel)) },
        {"LanguageLocation", (typeof(LanguageLocation), typeof(JunctionClassesViewModels.LanguageLocationViewModel)) },
        {"PlotPointChapter", (typeof(ChapterPlotPoint), typeof(JunctionClassesViewModels.ChapterPlotPointViewModel)) },
        { "Event", (typeof(Event), typeof(EventViewModel)) },
        { "Era", (typeof(Era), typeof(EraViewModel)) },
        { "Language", (typeof(Language), typeof(LanguageViewModel)) },
        { "Location", (typeof(Location), typeof(LocationViewModel)) },
        { "PriceExample", (typeof(PriceExample), typeof(PriceExample)) },
        { "River", (typeof(River), typeof(RiverViewModel)) },
        { "Route", (typeof(Route), typeof(RouteViewModel)) },
        { "Chapter", (typeof(Chapter), typeof(ChapterViewModel)) },
        { "Date", (typeof(Date), typeof(DateViewModel)) },
        { "Currency", (typeof(Currency), typeof(CurrencyViewModel)) }
    };

    public Dictionary<string, (Type, Type)> GetEntityMap() => _map;
}
