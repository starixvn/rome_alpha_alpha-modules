namespace Alpha.NetThrottling
{
    using System.Configuration;

    public class WhitelistConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("custIds", IsRequired = true)]
        public string CustIds
        {
            get
            {
                return (string)this["custIds"];
            }
        }

        public static WhitelistConfiguration Default
        {
            get
            {
                return Get("Alpha.NetThrottling.Whitelist");
            }
        }

        public static WhitelistConfiguration Get(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as WhitelistConfiguration;
        }
    }
}
