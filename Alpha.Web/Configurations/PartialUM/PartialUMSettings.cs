namespace Alpha.Web.Configurations.PartialUM
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    internal static class PartialUMSettings
    {
        private static Dictionary<string, FunctionElement> functionDataSource;

        internal static Dictionary<string, FunctionElement> FunctionDataSource
        {
            get
            {
                if (functionDataSource == null)
                {
                    functionDataSource = new Dictionary<string, FunctionElement>(StringComparer.OrdinalIgnoreCase);
                    var partialUMSection = ConfigurationManager.GetSection("PartialUMSection") as PartialUMSection;

                    foreach (FunctionElement function in partialUMSection.FunctionCollection)
                    {
                        foreach (SettingElement setting in function.SettingCollection)
                        {
                            functionDataSource[setting.SettingKey] = function;
                        }
                    }
                }

                return functionDataSource;
            }
        }

        internal static FunctionElement GetUMFunction(string settingKey)
        {
            FunctionDataSource.TryGetValue(settingKey, out var functionElement);

            return functionElement;
        }
    }
}