using System.Configuration;

namespace Sunplus.Agent.Model.Base.Configurations
{
    public class ProductCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ProductElement this[int index]
        {
            get
            {
                return (ProductElement)BaseGet(index);
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

        public new ProductElement this[string elementKey]
        {
            get
            {
                return (ProductElement)BaseGet(elementKey);
            }
        }

        public void Add(ProductElement element)
        {
            BaseAdd(element);
        }

        public void Remove(ProductElement element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(element.Name);
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

        public int IndexOf(ProductElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ProductElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProductElement)element).Name;
        }
    }
}
