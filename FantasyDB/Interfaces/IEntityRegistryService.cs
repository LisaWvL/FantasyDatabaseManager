using System.Collections.Generic;
using System;

public interface IEntityRegistryService
{
    Dictionary<string, (Type ModelType, Type ViewModelType)> GetEntityMap();
}
