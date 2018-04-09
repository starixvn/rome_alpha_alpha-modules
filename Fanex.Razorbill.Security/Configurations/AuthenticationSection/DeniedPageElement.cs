namespace Fanex.Razorbill.Security.Configurations.AuthenticationSection
{
    using System.Configuration;

    public class DeniedPageElement : ConfigurationElement
    {
        public DeniedPageElement()
        {
        }
        
        [ConfigurationProperty("url", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }
       
    }
}