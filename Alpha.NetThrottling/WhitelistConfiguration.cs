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

        public static WhitelistConfiguration Current
        {
            get
            {
                return ConfigurationManager.GetSection("Alpha.NetThrottling.Whitelist") as WhitelistConfiguration;
            }
        }
    }
}
