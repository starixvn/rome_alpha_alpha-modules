namespace Fanex.Age.Utility
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionUtil
    {
        public static object GetValue(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            return propertyInfo != null ? propertyInfo.GetValue(obj, null) : null;
        }

        public static PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            return obj.GetType()
                .GetProperties()
                .FirstOrDefault(propertyInfo
                    => string.Compare(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static bool HasPropertyInfo(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            return propertyInfo != null;
        }
    }
}
