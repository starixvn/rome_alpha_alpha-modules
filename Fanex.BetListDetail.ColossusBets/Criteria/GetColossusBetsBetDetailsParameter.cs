namespace Fanex.BetListDetail.ColossusBets.Criteria
{
    using System;

    public interface GetColossusBetsBetDetailsParameter
    {
        int CustId { get; set; }

        long Refno { get; set; }

        long EventId { get; set; }

        int RoleId { get; set; }

        long TransID { get; set; }

        int AccId { get; set; }

        DateTime WinLossDate { get; set; }

        bool IsValid();

    }

    public class GetColossusBetsBetDetailsParameterImpl : GetColossusBetsBetDetailsParameter
    {
        public int CustId { get; set; }

        public long Refno { get; set; }

        public long EventId { get; set; }

        public int RoleId { get; set; }

        public long TransID { get; set; }

        public int AccId { get; set; }

        public DateTime WinLossDate { get; set; }

        public bool IsValid() => Refno > 0;

    }
}
