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
        public GroupLevelCollection GroupLevel
        {
            get
            {
                return (GroupLevelCollection)base[string.Empty];
            }
        }

        [ConfigurationProperty("fullpermission", DefaultValue = "0", IsRequired = false)]
        public string FullPermission
        {
            get { return (string)this["fullpermission"]; }
            set { this["fullpermission"] = value.ToLower(); }
        }

        [ConfigurationProperty("BelongingParams", IsRequired = true)]
        public string BelongingParams
        {
            get { return (string)this["BelongingParams"]; }
            set { this["BelongingParams"] = value; }
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