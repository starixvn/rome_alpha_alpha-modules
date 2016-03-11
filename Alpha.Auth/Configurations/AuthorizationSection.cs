namespace Alpha.Auth.Configurations
{
    using System.Configuration;
    using System.Web;

    public class AuthorizationSection : ConfigurationSection
    {
        private static AuthorizationSection instance
           = ConfigurationManager.GetSection("AuthorizationSection") as AuthorizationSection;

        public static AuthorizationSection Instance
        {
            get
            {
                return instance;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public GroupPageAccessCollection GroupPageAccesses
        {
            get
            {
                return (GroupPageAccessCollection)base[string.Empty];
            }
        }

        [ConfigurationProperty("fullpermission", DefaultValue = "0", IsRequired = false)]
        public string FullPermission
        {
            get { return (string)this["fullpermission"]; }
            set { this["fullpermission"] = value.ToLower(); }
        }

        [ConfigurationProperty("CustIdQueryStringParamNameMapping", IsRequired = true)]
        public string CustIdQueryStringParamNameMapping
        {
            get { return (string)this["CustIdQueryStringParamNameMapping"]; }
            set { this["CustIdQueryStringParamNameMapping"] = value; }
        }

        public string[] CustIdQueryStringParamNames
        {
            get
            {
                return CustIdQueryStringParamNameMapping.Split(',');
            }
        }

        [ConfigurationProperty("CustIdJsonParamNameMapping", IsRequired = true, DefaultValue = "txtCustomerObject,newCustomer")]
        public string CustIdJsonParamNameMapping
        {
            get { return (string)this["CustIdJsonParamNameMapping"]; }
            set { this["CustIdJsonParamNameMapping"] = value; }
        }

        public string[] CustIdJsonParamNames
        {
            get
            {
                return CustIdJsonParamNameMapping.Split(',');
            }
        }

        [ConfigurationProperty("VirtualDirectory", IsRequired = false)]
        public string VirtualDirectory
        {
            get
            {
                var virtualDirectory = (string)this["VirtualDirectory"];
                if (string.IsNullOrEmpty(virtualDirectory))
                {
                    virtualDirectory = HttpRuntime.AppDomainAppVirtualPath.Substring(1);
                }

                return virtualDirectory;
            }

            set
            {
                this["VirtualDirectory"] = value;
            }
        }

        [ConfigurationProperty("IgnoreUrls", IsRequired = false, DefaultValue = "index.aspx")]
        public string IgnoreUrls
        {
            get { return (string)this["IgnoreUrls"]; }
            set { this["IgnoreUrls"] = value; }
        }

        [ConfigurationProperty("IndexUrl", IsRequired = true, DefaultValue = "~/Index.aspx")]
        public string IndexUrl
        {
            get { return (string)this["IndexUrl"]; }
            set { this["IndexUrl"] = value; }
        }

        [ConfigurationProperty("SignOutUrl", IsRequired = true, DefaultValue = "~/SignOut.aspx")]
        public string SignOutUrl
        {
            get { return (string)this["SignOutUrl"]; }
            set { this["SignOutUrl"] = value; }
        }

        [ConfigurationProperty("AccessDeniedUrl", IsRequired = true, DefaultValue = "~/AccessDenied.aspx")]
        public string AccessDeniedUrl
        {
            get { return (string)this["AccessDeniedUrl"]; }
            set { this["AccessDeniedUrl"] = value; }
        }
    }
}