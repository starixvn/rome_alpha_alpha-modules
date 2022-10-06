namespace Alpha.PartialUM.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.PartialUM.Configurations;

    internal static class PartialUMSectionHelper
    {
        /// <summary>
        /// key: SettingKey
        /// value: List of function name
        /// </summary>
        private static IDictionary<string, IList<string>> functions;

        private static IDictionary<string, IList<string>> Functions
        {
            get
            {
                if (functions == null)
                {
                    functions = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);

                    foreach (FunctionElement function in PartialUMSection.Instance.FunctionCollection)
                    {
                        foreach (SettingElement setting in function.SettingCollection)
                        {
                            if (!functions.TryGetValue(setting.SettingKey, out var functionNames))
                            {
                                functionNames = new List<string>();
                                functions[setting.SettingKey] = functionNames;
                            }

                            if (!functionNames.Any(f => f.Equals(function.Name, StringComparison.OrdinalIgnoreCase)))
                            {
                                functionNames.Add(function.Name);
                            }
                        }
                    }
                }

                return functions;
            }
        }

        public static IEnumerable<string> GetFunctionNames(string settingKey)
            => Functions.TryGetValue(settingKey, out var functionNames)
            ? functionNames
            : Enumerable.Empty<string>();
    }
}
