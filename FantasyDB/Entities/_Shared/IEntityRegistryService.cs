using System.Collections.Generic;
using System;


namespace FantasyDB.Entities._Shared
{
    public interface IEntityRegistryService
    {
        Dictionary<string, (Type ModelType, Type ViewModelType)> GetEntityMap();
    }
}