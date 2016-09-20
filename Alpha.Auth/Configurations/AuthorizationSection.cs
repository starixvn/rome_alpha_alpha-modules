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

        [ConfigurationProperty("custIdQueryStringParamNameMapping", IsRequired = true)]
        public string CustIdQueryStringParamNameMapping
        {
            get { return (string)this["custIdQueryStringParamNameMapping"]; }
            set { this["custIdQueryStringParamNameMapping"] = value; }
        }

        public string[] CustIdQueryStringParamNames
        {
            get
            {
                return CustIdQueryStringParamNameMapping.Split(',');
            }
        }

        [ConfigurationProperty("custIdJsonParamNameMapping", IsRequired = true, DefaultValue = "txtCustomerObject,newCustomer")]
        public string CustIdJsonParamNameMapping
        {
            get { return (string)this["custIdJsonParamNameMapping"]; }
            set { this["custIdJsonParamNameMapping"] = value; }
        }

        [ConfigurationProperty("cachedExpireTime", IsRequired = true, DefaultValue = 30)]
        public int CachedExpireTime
        {
            get { return (int)this["cachedExpireTime"]; }
            set { this["cachedExpireTime"] = value; }
        }

        public string[] CustIdJsonParamNames
        {
            get
            {
                return CustIdJsonParamNameMapping.Split(',');
            }
        }

        [ConfigurationProperty("ignoreUrls", IsRequired = false, DefaultValue = "index.aspx")]
        public string IgnoreUrls
        {
            get { return (string)this["ignoreUrls"]; }
            set { this["ignoreUrls"] = value; }
        }

        public string[] SplitedIgnoreUrls
        {
            get
            {
                return IgnoreUrls.Split(',');
            }
        }

        [ConfigurationProperty("indexUrl", IsRequired = false, DefaultValue = "~/Index.aspx")]
        public string IndexUrl
        {
            get { return (string)this["indexUrl"]; }
            set { this["indexUrl"] = value; }
        }

        [ConfigurationProperty("signOutUrl", IsRequired = false, DefaultValue = "~/SignOut.aspx")]
        public string SignOutUrl
        {
            get { return (string)this["signOutUrl"]; }
            set { this["signOutUrl"] = value; }
        }

        [ConfigurationProperty("accessDeniedUrl", IsRequired = false, DefaultValue = "~/AccessDenied.aspx")]
        public string AccessDeniedUrl
        {
            get { return (string)this["accessDeniedUrl"]; }
            set { this["accessDeniedUrl"] = value; }
        }
    }
}