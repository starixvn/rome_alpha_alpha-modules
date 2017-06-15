using System;
using System.Collections.Generic;

namespace Alpha.StaticData.Mappers
{
    public interface IRepositoryMapper
    {
        IDictionary<string, Type> Maps { get; }
    }
}
