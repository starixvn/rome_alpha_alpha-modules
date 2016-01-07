namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class PageAccessCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public PageAccessElement this[int index]
        {
            get
            {
                return (PageAccessElement)BaseGet(index);
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

        public new PageAccessElement this[string name]
        {
            get
            {
                return (PageAccessElement)BaseGet(name);
            }
        }

        public void Add(PageAccessElement authenticationLevelElement)
        {
            BaseAdd(authenticationLevelElement);
        }

        public void Remove(PageAccessElement authenticationLevelElement)
        {
            if (BaseIndexOf(authenticationLevelElement) >= 0)
            {
                BaseRemove(authenticationLevelElement.Id);
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

        public int IndexOf(PageAccessElement authenticationLevelElement)
        {
            return BaseIndexOf(authenticationLevelElement);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new PageAccessElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PageAccessElement)element).Id;
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
    }
}