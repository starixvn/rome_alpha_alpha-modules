namespace Alpha.NetThrottling
{
    using System.Configuration;

    public class NetThrottlingConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("prefix", IsRequired = true)]
        public string Prefix
        {
            get
            {
                return (string)this["prefix"];
            }
        }

        [ConfigurationProperty("keys", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NetThrottlingKeyElement))]
        public NetThrottlingKeyCollection Keys
        {
            get
            {
                return (NetThrottlingKeyCollection)this["keys"];
            }
        }

        public static NetThrottlingConfiguration Current
        {
            get
            {
                return ConfigurationManager.GetSection("Alpha.NetThrottling") as NetThrottlingConfiguration;
            }
        }
    }

    public class NetThrottlingKeyCollection : ConfigurationElementCollection
    {
        public new NetThrottlingKeyElement this[string name]
        {
            get
            {
                return (NetThrottlingKeyElement)BaseGet(name);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new NetThrottlingKeyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NetThrottlingKeyElement)element).Name;
        }
    }

    public class NetThrottlingKeyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
        }

        [ConfigurationProperty("method", IsRequired = true)]
        public string Method
        {
            get
            {
                return (string)this["method"];
            }
        }

        [ConfigurationProperty("maxRequests", IsRequired = true)]
        public int MaxRequests
        {
            get
            {
                return (int)this["maxRequests"];
            }
        }

        [ConfigurationProperty("periodInSeconds", IsRequired = true)]
        public int PeriodInSeconds
        {
            get
            {
                return (int)this["periodInSeconds"];
            }
        }

        [ConfigurationProperty("filter", IsRequired = true)]
        public string Filter
        {
            get
            {
                return (string)this["filter"];
            }
        }
    }
}