namespace Alpha.NetThrottling
{
    using System.Configuration;

    public class WhitelistConfiguration : ConfigurationSection
    {
        public static readonly WhitelistConfiguration Default = Get("Alpha.NetThrottling.Whitelist");

        [ConfigurationProperty("custIds", IsRequired = true)]
        public string CustIds
        {
            get
            {
                return (string)this["custIds"];
            }
        }

        public static WhitelistConfiguration Get(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as WhitelistConfiguration;
        }
    }
}
