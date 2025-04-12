// File: Attributes/HandlesJunctionAttribute.cs
using System;

namespace FantasyDB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HandlesJunctionAttribute(string junctionEntity, string thisKey, string foreignKey) : Attribute
    {
        public string JunctionEntity { get; } = junctionEntity;
        public string ThisKey { get; } = thisKey;
        public string ForeignKey { get; } = foreignKey;
    }
}