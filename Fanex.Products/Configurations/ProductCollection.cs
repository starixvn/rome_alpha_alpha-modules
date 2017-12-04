using System.Configuration;

namespace Fanex.Products.Configurations
{
    public class ProductCollection : ConfigurationElementCollection
    {
        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ProductElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProductElement)element).Id;
        }
    }
}
