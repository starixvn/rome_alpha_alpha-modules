namespace Alpha.Web.Configurations.PartialUM
{
    using System.Collections.Generic;
    using System.Configuration;

    public static class PartialUMSettings
    {
        private static PartialUMSection partialUMSection
            = ConfigurationManager.GetSection("PartialUMSection") as PartialUMSection;
        private static Dictionary<string, FunctionElement> functionDataSource;

        private static Dictionary<string, FunctionElement> FunctionDataSource
        {
            get
            {
                if (functionDataSource == null)
                {
                    var functionDataSource = new Dictionary<string, FunctionElement>();

                    foreach (FunctionElement function in partialUMSection.FunctionCollection)
                    {
                        foreach (SettingElement setting in function.SettingCollection)
                        {
                            functionDataSource.Add(setting.SettingKey, function);
                        }
                    }
                }

                return functionDataSource;
            }
        }

        public static FunctionElement GetUMFunction(string settingKey) => FunctionDataSource[settingKey];

        public static bool IsUMFunction(string settingKey) => FunctionDataSource.ContainsKey(settingKey);
    }
}