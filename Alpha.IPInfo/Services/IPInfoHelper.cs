using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Alpha.IPInfo.Services
{
    internal static class IPInfoHelper
    {
        public static Tuple<IList<string>, IList<string>> SplitIPV4AndIPV6List(IEnumerable<string> ipList)
        {
            IList<string> ipV4List = new List<string>();
            IList<string> ipV6List = new List<string>();

            foreach (var ip in ipList)
            {
                if (IsValid(ip, out var ipAddress))
                {
                    if (IsIPV6(ipAddress))
                    {
                        ipV6List.Add(ip);
                    }
                    else
                    {
                        ipV4List.Add(ip);
                    }
                }
            }

            return Tuple.Create(ipV4List, ipV6List);
        }

        private static bool IsValid(string ip, out IPAddress ipAddress)
            => IPAddress.TryParse(ip, out ipAddress);

        private static bool IsIPV6(IPAddress ipAddress)
            => ipAddress.AddressFamily == AddressFamily.InterNetworkV6;

        public static long ConvertIPV4ToNumber(string ip)
        {
            if (IPAddress.TryParse(ip, out var ipAddress))
            {
                var ipAddressBytes = ipAddress.GetAddressBytes();
                Array.Reverse(ipAddressBytes);

                return BitConverter.ToUInt32(ipAddressBytes, 0);
            }

            return 0;
        }
    }
}