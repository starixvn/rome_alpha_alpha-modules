namespace Alpha.IPInfo.Models
{
    public class IPInformationConfig
    {
        public string IPV4TopCountries { get; set; }

        public int IPV4TopCountriesCacheInMinutes { get; set; } = 60 * 24;

        public int IPV4CacheInMinutes { get; set; } = 60;

        public int IPV6CacheInMinutes { get; set; } = 10;
    }
}