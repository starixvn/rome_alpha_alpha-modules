namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;
    using System.Web;

    public class AuthenticationSection : ConfigurationSection
    {
        private static AuthenticationSection instance
           = ConfigurationManager.GetSection("AuthenticationSection") as AuthenticationSection;

        public static AuthenticationSection Instance
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
    }
}