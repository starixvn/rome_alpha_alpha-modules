namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class GroupPageAccessElement : ConfigurationElement
    {
        [ConfigurationProperty("roleid", DefaultValue = 2, IsRequired = false, IsKey = false)]
        public int RoleId
        {
            get { return (int)this["roleid"]; }
            set { this["roleid"] = value; }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("PageAccesses", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PageAccessCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PageAccessCollection PageAccesses
        {
            get
            {
                return (PageAccessCollection)base["PageAccesses"];
            }
        }

        [ConfigurationProperty("DeniedPages", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DeniedPageCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public DeniedPageCollection DeniedPages
        {
            get
            {
                return (DeniedPageCollection)base["DeniedPages"];
            }
        }
    }
}