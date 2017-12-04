using System.Configuration;

namespace Fanex.Products.Configurations
{
    public class ProductElement : ConfigurationElement
    {
        [ConfigurationProperty("id", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public int Id
        {
            get
            {
                return (int)this["id"];
            }

            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }

            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("active", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool Active
        {
            get
            {
                return (bool)this["active"];
            }

            set
            {
                this["active"] = value;
            }
        }

        [ConfigurationProperty("sportId", DefaultValue = "-1", IsRequired = false, IsKey = false)]
        public int SportId
        {
            get
            {
                return (int)this["sportId"];
            }

            set
            {
                this["sportId"] = value;
            }
        }

        [ConfigurationProperty("reportSPParam", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string ReportSPParam
        {
            get
            {
                return this["reportSPParam"].ToString();
            }

            set
            {
                this["reportSPParam"] = value;
            }
        }

        [ConfigurationProperty("viewPTLink", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string ViewPTLink
        {
            get
            {
                return this["viewPTLink"].ToString();
            }

            set
            {
                this["viewPTLink"] = value;
            }
        }

        [ConfigurationProperty("viewCommLink", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string ViewCommLink
        {
            get
            {
                return this["viewCommLink"].ToString();
            }

            set
            {
                this["viewCommLink"] = value;
            }
        }

        [ConfigurationProperty("customerSettingLink", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string CustomerSettingLink
        {
            get
            {
                return this["customerSettingLink"].ToString();
            }

            set
            {
                this["customerSettingLink"] = value;
            }
        }

        [ConfigurationProperty("enableStatusLink", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string EnableStatusLink
        {
            get
            {
                return this["enableStatusLink"].ToString();
            }

            set
            {
                this["enableStatusLink"] = value;
            }
        }

        [ConfigurationProperty("syncCustomerLink", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SyncCustomerLink
        {
            get
            {
                return this["syncCustomerLink"].ToString();
            }

            set
            {
                this["syncCustomerLink"] = value;
            }
        }

        [ConfigurationProperty("statusHandlerType", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string StatusHandlerType
        {
            get
            {
                return this["statusHandlerType"].ToString();
            }

            set
            {
                this["statusHandlerType"] = value;
            }
        }

        [ConfigurationProperty("microSiteNameAppSetting", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string MicroSiteNameAppSetting
        {
            get
            {
                return this["microSiteNameAppSetting"].ToString();
            }

            set
            {
                this["microSiteNameAppSetting"] = value;
            }
        }
    }
}
