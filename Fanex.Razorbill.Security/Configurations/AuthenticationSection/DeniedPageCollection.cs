namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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

        /// <summary>
        /// Remove element.
        /// </summary>
        /// <param name="authenticationLevelElement">Authentication Level Element.</param>
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

        /// <summary>
        /// Get Index of.
        /// </summary>
        /// <param name="authenticationLevelElement">Authentication Level Element.</param>
        /// <returns>Return index.</returns>
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