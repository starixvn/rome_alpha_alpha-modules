namespace Fanex.BetListDetail.ColossusBets.Criteria
{
    public interface GetSiteIdByCustomerIdParameter
    {
        int CustomerId { get; set; }

        int SiteId { get; set; }

        bool IsValid();
    }

    public class GetSiteIdByCustomerIdParameterImpl : GetSiteIdByCustomerIdParameter
    {
        public int CustomerId { get; set; }

        public int SiteId { get; set; }

        public bool IsValid() => CustomerId > 0;
    }
}
