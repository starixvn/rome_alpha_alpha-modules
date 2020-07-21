namespace Alpha.NetThrottling.Filters
{
    using System;
    using System.Collections.Generic;

    public static class FilterFactory
    {
        private static readonly IDictionary<string, IFilter> cache = new Dictionary<string, IFilter>();

        public static IFilter Create(string typeName)
        {
            if (!cache.ContainsKey(typeName))
            {
                lock (cache)
                {
                    if (!cache.ContainsKey(typeName))
                    {
                        return InternalCreate(typeName);
                    }
                }
            }

            return cache[typeName];
        }

        private static IFilter InternalCreate(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new InvalidCastException($"{typeName} is invalid");
            }

            var instance = Activator.CreateInstance(type);
            if (!(instance is IFilter filter))
            {
                throw new InvalidCastException($"{typeName} is not a type of {typeof(IFilter)}");
            }

            cache.Add(typeName, filter);
            return filter;
        }
    }
}
