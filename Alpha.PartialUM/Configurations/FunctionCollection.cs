namespace Alpha.PartialUM.Configurations
{
    using System.Configuration;

    internal sealed class FunctionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new FunctionElement();

        protected override object GetElementKey(ConfigurationElement element) => ((FunctionElement)element).Name;

        public new FunctionElement this[string key] => (FunctionElement)BaseGet(key);
    }
}
