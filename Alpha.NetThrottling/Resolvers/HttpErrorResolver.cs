namespace Alpha.NetThrottling.Resolvers
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class HttpErrorResolver : BaseResolver
    {
        public override void BlockNonAjaxRequest(HttpContextBase context)
        {
            throw new HttpException(HttpStatusCodes.TooManyRequests, Lang.BlockedContent);
        }
    }
}
