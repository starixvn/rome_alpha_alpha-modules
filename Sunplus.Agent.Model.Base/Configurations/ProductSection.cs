using System.Configuration;

namespace Sunplus.Agent.Model.Base.Configurations
{
    public class ProductSection : ConfigurationSection
    {
        public static ProductSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("ProductSection") as ProductSection;
            }
        }

        [ConfigurationProperty("Products", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProductCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ProductCollection Products
        {
            get
            {
                return (ProductCollection)base["Products"];
            }
        }
    }
}
