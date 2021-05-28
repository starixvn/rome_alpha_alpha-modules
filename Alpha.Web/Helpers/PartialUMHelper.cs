namespace Alpha.Web.Helpers
{
    using Configurations.PartialUM;
    using Models.PartialUM;

    public static class PartialUMHelper
    {
        public static FunctionModel GetUMFunction(string settingKey)
        {
            var function = PartialUMSettings.GetUMFunction(settingKey);

            return function == null
                ? null
                : new FunctionModel(function.Name, function.UMFrom, function.UMTo);
        }
    }
}