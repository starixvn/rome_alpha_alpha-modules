namespace Alpha.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Configurations.PartialUM;
    using Models.PartialUM;
    using SettingElement = Configurations.PartialUM.SettingElement;

    public static class PartialUMHelper
    {
        private static IDictionary<string, FunctionModel> functionDataSource;

        private static IDictionary<string, FunctionModel> FunctionDataSource
        {
            get
            {
                if (functionDataSource == null)
                {
                    functionDataSource = new Dictionary<string, FunctionModel>(StringComparer.OrdinalIgnoreCase);
                    var partialUMSection = ConfigurationManager.GetSection("PartialUMSection") as PartialUMSection;

                    foreach (FunctionElement function in partialUMSection.FunctionCollection)
                    {
                        if (function.IsActive)
                        {
                            foreach (SettingElement setting in function.SettingCollection)
                            {
                                functionDataSource[setting.SettingKey] = new FunctionModel(function.UMFrom, function.UMTo);
                            }
                        }   
                    }
                }

                return functionDataSource;
            }
        }

        public static FunctionModel GetUMFunction(string settingKey)
        {
            FunctionDataSource.TryGetValue(settingKey, out var function);

            return function;
        }
    }
}