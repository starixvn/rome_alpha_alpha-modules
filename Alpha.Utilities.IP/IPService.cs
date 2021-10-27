namespace Alpha.Utilities.IP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Numerics;
    using System.Text.RegularExpressions;
    using Alpha.Utilities.IP.Models;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Model;

    public interface IPService
    {
        bool IsVNIP(string ipAddress);

        FormatedIP FormatIP(string ip, bool isAdminCanViewVNIP);

        IPInformationModel GetIPInformation(string ip);
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
            var ipVnRange = GetVNIPs();

            var isIpv6 = IsIPV6(ipAddress);
            if (isIpv6)
            {
                var ipv6Number = ConvertIpv6ToNumber(ipAddress);
                return ipVnRange.Item2.Any(ip => ip.FromNumber <= ipv6Number && ipv6Number <= ip.ToNumber);
            }

            double ipNumber = ConvertIPToNumber(ipAddress);
            return ipVnRange.Item1.Any(ip => ip.From <= ipNumber && ipNumber <= ip.To);
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

        public IPInformationModel GetIPInformation(string ip)
            => dynamicRepository.Get<IPInformationModel>(new GetIPInformationCriteria(ip));

        private Tuple<IEnumerable<Ipv4Range>, IEnumerable<Ipv6Range>> GetVNIPs()
        {
            var cache = new CacheService();

            var vietnamIPs = cache.GetOrSet(
                VNIPListCacheKey,
                () => dynamicRepository.FetchMultiple<Ipv4Range, Ipv6Range>(new GetIPVNCriteria()));

            return vietnamIPs;
        }

        private bool IsIPV6(string clientIP)
            => IPAddress.TryParse(clientIP, out IPAddress address)
                && address.AddressFamily == AddressFamily.InterNetworkV6;

        private long ConvertIPToNumber(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress parsedIpAddress))
            {
                var addressBytes = parsedIpAddress.GetAddressBytes();
                Array.Reverse(addressBytes);

                return BitConverter.ToUInt32(addressBytes, 0);
            }

            return 0;
        }

        public static BigInteger ConvertIpv6ToNumber(string ipv6)
        {
            BigInteger bigInteger = 0;
            if (IPAddress.TryParse(ipv6, out IPAddress address))
            {
                byte[] numArray = address.GetAddressBytes();
                if (BitConverter.IsLittleEndian)
                {
                    List<byte> byteList = new List<byte>(numArray);
                    byteList.Reverse();
                    numArray = byteList.ToArray();
                }
                bigInteger = numArray.Length <= 8 ? BitConverter.ToUInt32(numArray, 0) : ((BigInteger)BitConverter.ToUInt64(numArray, 8) << 64) + BitConverter.ToUInt64(numArray, 0);
            }

            return bigInteger;
        }

        private static bool IsValidIP(string ipAddress)
        {
            IPAddress parsedIpAddress;
            return !string.IsNullOrWhiteSpace(ipAddress)
                && IPAddress.TryParse(ipAddress, out parsedIpAddress);
        }
    }
}