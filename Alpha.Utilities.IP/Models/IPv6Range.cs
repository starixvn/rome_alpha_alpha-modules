using System.Globalization;
using System.Numerics;

namespace Alpha.Utilities.IP.Model
{
    public class Ipv6Range
    {
        public string From { get; set; }

        public string To { get; set; }

        public BigInteger FromNumber
            => BigInteger.Parse(From, NumberStyles.AllowHexSpecifier);

        public BigInteger ToNumber
            => BigInteger.Parse(To, NumberStyles.AllowHexSpecifier);

        public bool IsManual { get; set; }
    }
}