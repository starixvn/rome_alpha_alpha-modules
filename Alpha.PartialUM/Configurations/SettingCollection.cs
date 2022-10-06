namespace Alpha.PartialUM.Configurations
{
    using System.Configuration;

    internal sealed class SettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new SettingElement();

        protected override object GetElementKey(ConfigurationElement element) => ((SettingElement)element).SettingKey;

        public new SettingElement this[string key] => (SettingElement)BaseGet(key);
    }
}
