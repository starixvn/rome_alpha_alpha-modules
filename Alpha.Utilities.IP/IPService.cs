namespace Alpha.Utilities.IP
{
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    public interface IPService
    {
        bool IsVNIP(string ipAddress);
        FormatedIP FormatIP(string ip, bool isAdminCanViewVNIP);
    }

    public class IPServiceImpl : IPService
    {
        private const string IPValidateRegex = @"(?:(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d\d?)";
        private const string VNIPListCacheKey = "Alpha.BetLists.Service.IPService.VNIPList";
        private readonly IDynamicRepository dynamicRepository;

        public IPServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public bool IsVNIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }

            double ipNumber = ConvertIPToNumber(ipAddress);
            var ipVnRange = GetVNIPs();

            return ipVnRange.Any(ip => ip.From <= ipNumber && ipNumber <= ip.To);
        }

        public FormatedIP FormatIP(string ip, bool isAdminCanViewVNIP)
        {
            if (!IsValidIP(ip))
            {
                return new FormatedIP(ip, false);
            }

            Regex regex = new Regex(IPValidateRegex);
            if (!regex.IsMatch(ip, 0))
            {
                return new FormatedIP(ip, false);
            }

            if (IsVNIP(ip) && !isAdminCanViewVNIP)
            {
                return new FormatedIP(string.Empty, false);
            }

            return new FormatedIP(ip, true);
        }

        private IEnumerable<IPRange> GetVNIPs()
        {
            var cache = new CacheService();

            var vietnamIPs = cache.GetOrSet(
                VNIPListCacheKey,
                () => dynamicRepository.Fetch<IPRange>(new GetIPVNCriteria()));

            return vietnamIPs;
        }

        private long ConvertIPToNumber(string ipAddress)
        {
            IPAddress parsedIpAddress;

            if (IPAddress.TryParse(ipAddress, out parsedIpAddress))
            {
                var addressBytes = parsedIpAddress.GetAddressBytes();
                Array.Reverse(addressBytes);

                return BitConverter.ToUInt32(addressBytes, 0);
            }

            return 0;
        }

        private static bool IsValidIP(string ipAddress)
        {
            IPAddress parsedIpAddress;
            return !string.IsNullOrWhiteSpace(ipAddress)
                && IPAddress.TryParse(ipAddress, out parsedIpAddress);
        }
    }
}