namespace Alpha.Web.Configurations.PartialUM
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class PartialUMSettings
    {
        private static PartialUMSection partialUMSection
            = ConfigurationManager.GetSection("PartialUMSection") as PartialUMSection;

        public static IEnumerable<SettingElement> GetUMSps(string functionName)
        {
            return (
                from SettingElement element
                in partialUMSection.FunctionCollection[functionName].SettingCollection
                select element);
        }

        public static FunctionElement GetUMFunction(string functionName)
            => partialUMSection.FunctionCollection[functionName];
    }
}