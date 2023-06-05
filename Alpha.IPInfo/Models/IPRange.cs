using System;

namespace Alpha.IPInfo.Models
{
    /// <summary>
    /// https://stackoverflow.com/questions/461439/how-would-i-search-a-range-of-ranged-values-using-c-sharp
    /// </summary>
    internal sealed class IPRange : IComparable<IPRange>
    {
        public IPRange(long lower, long upper, string country)
        {
            Lower = lower;
            Upper = upper;
            Country = country;
        }

        public long Lower { get; }

        public long Upper { get; }

        public string Country { get; }

        public int CompareTo(IPRange other)
        {
            if (Lower < other.Lower && Upper < other.Upper)
            {
                return -1;
            }

            if (Lower > other.Lower && Upper > other.Upper)
            {
                return 1;
            }

            if (Lower == other.Lower && Upper == other.Upper)
            {
                return 0;
            }

            throw new ArgumentException("Incomparable values (overlapping)");
        }

        public int CompareTo(long value)
        {
            if (value < Lower)
            {
                return 1;
            }

            if (value > Upper)
            {
                return -1;
            }

            return 0;
        }
    }
}
