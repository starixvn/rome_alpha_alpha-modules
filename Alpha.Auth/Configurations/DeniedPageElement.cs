namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public class DeniedPageElement : ConfigurationElement
    {        
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }
    }
}