using System.Collections.Generic;
using Alpha.IPInfo.Models;

namespace Alpha.IPInfo.Extensions
{
    internal static class ListExtensions
    {
        public static IPRange BinarySearch2(this IList<IPRange> ipRanges, long value)
        {
            int min = 0;
            int max = ipRanges.Count - 1;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                IPRange ipRange = ipRanges[mid];
                int comparison = ipRange.CompareTo(value);

                if (comparison == 0)
                {
                    return ipRange;
                }

                if (comparison < 0)
                {
                    min = mid + 1;
                }
                else //// if (comparison > 0)
                {
                    max = mid - 1;
                }
            }

            return null;
        }

        public static string JoinComma(this IList<string> values)
            => string.Join(",", values);
    }
}