namespace Alpha.Web.Helpers
{
    using Configurations.PartialUM;
    using Models.PartialUM;
    using System;
    using System.Linq;

    public static class PartialUMHelper
    {
        public static bool IsUMSpName(string functionName, string spName)
            => PartialUMSettings.GetUMSps(functionName)
            .Select(sp => sp.Name)
            .Contains(spName, StringComparer.CurrentCultureIgnoreCase);

        public static FunctionModel GetUMFunction(string functionName)
        {
            var function = PartialUMSettings.GetUMFunction(functionName);
            return new FunctionModel(function.Name, function.UMFrom, function.UMTo);
        }
    }
}