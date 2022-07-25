namespace Alpha.PartialUM.Configurations
{
    using System.Configuration;

    internal sealed class PartialUMSection : ConfigurationSection
    {
        public static PartialUMSection Instance { get; }
            = ConfigurationManager.GetSection("PartialUMSection") as PartialUMSection;

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FunctionCollection), AddItemName = "Function")]
        public FunctionCollection FunctionCollection => (FunctionCollection)base[""];
    }
}
