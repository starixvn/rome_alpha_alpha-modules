namespace Alpha.Web.Configurations.PartialUM
{
    using System.Configuration;

    public sealed class PartialUMSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FunctionCollection), AddItemName = "Function")]
        public FunctionCollection FunctionCollection => (FunctionCollection)base[""];
    }

    public sealed class FunctionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new FunctionElement();

        protected override object GetElementKey(ConfigurationElement element) => ((FunctionElement)element).Name;

        public new FunctionElement this[string key] => (FunctionElement)BaseGet(key);
    }

    public sealed class FunctionElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get => this["Name"].ToString();
            set => this["Name"] = value;
        }

        [ConfigurationProperty("UMFrom", IsRequired = false, DefaultValue = "")]
        public string UMFrom
        {
            get => this["UMFrom"].ToString();
            set => this["UMFrom"] = value;
        }

        [ConfigurationProperty("UMTo", IsRequired = false, DefaultValue = "")]
        public string UMTo
        {
            get => this["UMTo"].ToString();
            set => this["UMTo"] = value;
        }

        [ConfigurationProperty("IsActive", IsRequired = false, DefaultValue = false)]
        public bool IsActive
        {
            get => (bool)this["IsActive"];
            set => this["IsActive"] = value;
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SettingCollection), AddItemName = "Add")]
        public SettingCollection SettingCollection => (SettingCollection)base[""];
    }

    public sealed class SettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new SettingElement();

        protected override object GetElementKey(ConfigurationElement element) => ((SettingElement)element).SettingKey;

        public new SettingElement this[string key] => (SettingElement)BaseGet(key);
    }

    public sealed class SettingElement : ConfigurationElement
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