namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class GroupLevelElement : ConfigurationElement
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

        [ConfigurationProperty("PageAccess", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PageAccessCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PageAccessCollection GroupLevel
        {
            get
            {
                return (PageAccessCollection)base["PageAccess"];
            }
        }
    }
}