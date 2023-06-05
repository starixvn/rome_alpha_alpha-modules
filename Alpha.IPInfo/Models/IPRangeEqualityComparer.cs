using System.Collections.Generic;

namespace Alpha.IPInfo.Models
{
    internal class IPRangeEqualityComparer : IEqualityComparer<IPRange>
    {
        public bool Equals(IPRange x, IPRange y)
            => x.Lower == y.Lower && x.Upper == y.Upper;


        public int GetHashCode(IPRange obj)
            => obj.GetHashCode();
    }
}