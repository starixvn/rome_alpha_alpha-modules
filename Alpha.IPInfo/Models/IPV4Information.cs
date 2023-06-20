namespace Alpha.IPInfo.Models
{
    public class IPV4Information
    {
        public IPV4Information()
        {
        }

        public IPV4Information(
            string ip,
            long ipFrom,
            long ipTo,
            string countryShort,
            string countryLong)
        {
            IP = ip;
            IPFrom = ipFrom;
            IPTo = ipTo;
            CountryShort = countryShort;
            CountryLong = countryLong;
        }

        public string IP { get; }

        public long IPFrom { get; }

        public long IPTo { get; }

        public string CountryShort { get; }

        public string CountryLong { get; }
    }
}
