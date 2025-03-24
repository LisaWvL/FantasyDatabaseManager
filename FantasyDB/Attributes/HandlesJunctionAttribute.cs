// File: Attributes/HandlesJunctionAttribute.cs
using System;

namespace FantasyDB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HandlesJunctionAttribute : Attribute
    {
        public string JunctionEntity { get; }
        public string ThisKey { get; }
        public string ForeignKey { get; }

        public HandlesJunctionAttribute(string junctionEntity, string thisKey, string foreignKey)
        {
            JunctionEntity = junctionEntity;
            ThisKey = thisKey;
            ForeignKey = foreignKey;
        }
    }
}