namespace Alpha.NetThrottling.Utils
{
    using System;
    using System.Collections.Generic;

    public static class TypeFactory<T>
    {
        private static readonly IDictionary<string, T> cache = new Dictionary<string, T>();

        public static T Create(string typeName)
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

        private static T InternalCreate(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new InvalidCastException($"{typeName} is invalid");
            }

            var instance = Activator.CreateInstance(type);
            if (!(instance is T filter))
            {
                throw new InvalidCastException($"{typeName} is not a type of {typeof(T)}");
            }

            cache.Add(typeName, filter);
            return filter;
        }
    }
}
