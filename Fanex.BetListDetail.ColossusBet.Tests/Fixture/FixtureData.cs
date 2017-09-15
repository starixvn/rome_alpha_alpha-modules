namespace Fanex.BetListDetail.ColossusBets.Tests
{
    using Fanex.BetListDetail.ColossusBets.Configurations;
    using System.Collections.Generic;

    public static class FixtureData
    {
        public static void Init()
        {
            AppSettings.ColossusBetsConfig = new ColossusBets()
            {
                SiteIds = "3100100;3200100;3300100",
                GetMemberChoiceAddress = "/api/tickets/ref/{0}/selection?productToken={1}&amp;siteID={2}",
                GetTokenAddress = "http://220.130.26.188:30051/api/Authorize/AuthorizeCode?Vendor=101&amp;VendorKey=WZWtzDfCJW&amp;CustomerId={0}&amp;vendorProduct=4&amp;memberID={1}",
                Domains = new List<DomainInformation>
                {
                    new DomainInformation { SiteId="37", Domain= "http://spondemo.staging-colossusbets.com" },
                    new DomainInformation {  SiteId="30", Domain= "http://12bet.staging-colossusbets.com"}
                }
            };
        }
    }
}
