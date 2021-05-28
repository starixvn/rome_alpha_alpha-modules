namespace Alpha.Web.Configurations.PartialUM
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    internal static class PartialUMSettings
    {
        internal static IDictionary<string, FunctionElement> FunctionDataSource = GetFunctionDataSource();

        internal static FunctionElement GetUMFunction(string settingKey)
        {
            FunctionDataSource.TryGetValue(settingKey, out var functionElement);

            return functionElement;
        }

        internal static bool IsUMFunction(string settingKey) => FunctionDataSource.ContainsKey(settingKey);

        private static IDictionary<string, FunctionElement> GetFunctionDataSource()
        {
            var functions = new Dictionary<string, FunctionElement>(StringComparer.OrdinalIgnoreCase);
            var partialUMSection = ConfigurationManager.GetSection("PartialUMSection") as PartialUMSection;

            foreach (FunctionElement function in partialUMSection.FunctionCollection)
            {
                foreach (SettingElement setting in function.SettingCollection)
                {
                    functions[setting.SettingKey] = function;
                }
            }

            return functions;
        }
    }
}