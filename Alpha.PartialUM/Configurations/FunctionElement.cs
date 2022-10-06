namespace Alpha.PartialUM.Configurations
{
    using System.Configuration;

    internal sealed class FunctionElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => this["Name"].ToString();
            set => this["Name"] = value;
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SettingCollection), AddItemName = "Add")]
        public SettingCollection SettingCollection => (SettingCollection)base[""];
    }
}
