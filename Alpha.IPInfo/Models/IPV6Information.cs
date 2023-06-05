namespace Alpha.IPInfo.Models
{
    public class IPV6Information
    {
        public IPV6Information()
        {
        }

        public IPV6Information(
            string ip,
            byte[] ipFrom,
            byte[] ipTo,
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

        public byte[] IPFrom { get; }

        public byte[] IPTo { get; }

        public string CountryShort { get; }

        public string CountryLong { get; }
    }
}
