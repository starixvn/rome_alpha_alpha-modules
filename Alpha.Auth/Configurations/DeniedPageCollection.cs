namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class DeniedPageCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public DeniedPageElement this[int index]
        {
            get
            {
                return (DeniedPageElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new DeniedPageElement this[string name]
        {
            get
            {
                return (DeniedPageElement)BaseGet(name);
            }
        }

        public void Add(DeniedPageElement authenticationLevelElement)
        {
            BaseAdd(authenticationLevelElement);
        }

        public void Remove(DeniedPageElement authenticationLevelElement)
        {
            if (BaseIndexOf(authenticationLevelElement) >= 0)
            {
                BaseRemove(authenticationLevelElement.Url);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(DeniedPageElement authenticationLevelElement)
        {
            return BaseIndexOf(authenticationLevelElement);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new DeniedPageElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DeniedPageElement)element).Url;
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
    }
}