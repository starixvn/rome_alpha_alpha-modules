namespace Fanex.BetListDetail.ColossusBets.Api
{
    using System;
    using System.Net;

    public interface ColossusBetsWebClient
    {
        string DownloadString(Uri address);
    }

    public class ColossusBetsWebClientImpl : ColossusBetsWebClient
    {
        public string DownloadString(Uri address)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(address);
            }
        }
    }
}
