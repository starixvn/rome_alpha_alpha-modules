namespace Alpha.PartialUM.Configurations
{
    using System.Configuration;

    internal sealed class SettingElement : ConfigurationElement
    {
        [ConfigurationProperty("SettingKey", IsKey = true, IsRequired = true)]
        public string SettingKey
        {
            get => this["SettingKey"].ToString();
            set => this["SettingKey"] = value;
        }

        [ConfigurationProperty("SpName", IsRequired = false, DefaultValue = "")]
        public string SpName
        {
            get => this["SpName"].ToString();
            set => this["SpName"] = value;
        }
    }
}
