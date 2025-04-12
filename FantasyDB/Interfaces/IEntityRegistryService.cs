using System.Collections.Generic;
using System;

namespace FantasyDB.Interfaces
{
    public interface IEntityRegistryService
    {
        Dictionary<string, (Type ModelType, Type ViewModelType)> GetEntityMap();
    }
}