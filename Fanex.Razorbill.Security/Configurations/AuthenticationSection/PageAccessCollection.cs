namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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

        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="authenticationLevelElement">Authentication Level Element.</param>
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

        /// <summary>
        /// Get Index of.
        /// </summary>
        /// <param name="authenticationLevelElement">Authentication Level Element.</param>
        /// <returns>Return index.</returns>
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