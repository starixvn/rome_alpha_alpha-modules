using System.Collections.Generic;

namespace Alpha.StaticData.Mappers
{
    public interface IDataModelMapper
    {
        IDictionary<string, string> Maps { get; }
    }
}
