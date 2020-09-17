namespace Alpha.NetThrottling
{
    using System.Configuration;

    public sealed class NetThrottlingConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("resolver", IsRequired = true)]
        public string Resolver
        {
            get
            {
                return (string)this["resolver"];
            }
        }

        [ConfigurationProperty("redisConfigurationOptionsKey", IsRequired = true)]
        public string RedisConfigurationOptionsKey
        {
            get
            {
                return (string)this["redisConfigurationOptionsKey"];
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

        public static NetThrottlingConfiguration Default
        {
            get
            {
                return Get("Alpha.NetThrottling");
            }
        }

        public static NetThrottlingConfiguration Get(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as NetThrottlingConfiguration;
        }
    }

    public sealed class NetThrottlingKeyCollection : ConfigurationElementCollection
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

    public sealed class NetThrottlingKeyElement : ConfigurationElement
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

        [ConfigurationProperty("throttleDuration", IsRequired = true)]
        public int ThrottleDuration
        {
            get
            {
                return (int)this["throttleDuration"];
            }
        }

        [ConfigurationProperty("blockDuration", IsRequired = true)]
        public int BlockDuration
        {
            get
            {
                return (int)this["blockDuration"];
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