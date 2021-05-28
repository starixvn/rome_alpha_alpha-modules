namespace Alpha.Web.Helpers
{
    using Configurations.PartialUM;
    using Models.PartialUM;
    using System;
    using System.Linq;

    public static class PartialUMHelper
    {
        public static bool IsUMFunction(string settingKey) => PartialUMSettings.IsUMFunction(settingKey);

        public static FunctionModel GetUMFunction(string settingKey)
        {
            var function = PartialUMSettings.GetUMFunction(settingKey);
            return new FunctionModel(function.Name, function.UMFrom, function.UMTo);
        }
    }
}