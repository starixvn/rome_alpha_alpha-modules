namespace Alpha.SecurityCode.Configurations
{
    using System.Configuration;

    public class IgnoreUrlElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
        }

        [ConfigurationProperty("isFullUrl", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool IsFullUrl
        {
            get
            {
                return (bool)this["isFullUrl"];
            }
        }
    }
}