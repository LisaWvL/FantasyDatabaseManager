using FantasyDB.Models;
using FantasyDB.ViewModels;
using System.Collections.Generic;
using System;

public class EntityRegistryService : IEntityRegistryService
{
    private static readonly Dictionary<string, (Type, Type)> _map = new()
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

    public Dictionary<string, (Type, Type)> GetEntityMap() => _map;
}
