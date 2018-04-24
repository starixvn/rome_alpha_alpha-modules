namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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