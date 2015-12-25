namespace Alpha.SecurityCode.Configurations
{
    using System.Configuration;

    public class IgnoreUrlCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public IgnoreUrlElement this[int index]
        {
            get
            {
                return (IgnoreUrlElement)BaseGet(index);
            }
        }

        public new IgnoreUrlElement this[string alias]
        {
            get
            {
                return (IgnoreUrlElement)BaseGet(alias);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoreUrlElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnoreUrlElement)element).Url;
        }
    }
}