using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Entities._Shared;



namespace FantasyDB.Features.RelationshipGraph
{
    public class RelationshipGraph
    {
        //I want this view to return all data necessary, to display a full relationship graph for a given entity
        //This will be a complex query, that will return all related entities, and their relationships
        //This will be a recursive function, that will return all related entities, and their relationships
        //it will include styling information for display purposes like color coding 
        public int Id { get; set; }
        public string? Label { get; set; }
        public string? Type { get; set; }
        public List<int>? RelatedIds { get; set; }
        public string? Color { get; set; }
        public Dictionary<string, string>? Meta { get; set; }

        //List<RelationshipGraph>()
        //{
        //    return new List<RelationshipGraph>
        //}

    }
}
