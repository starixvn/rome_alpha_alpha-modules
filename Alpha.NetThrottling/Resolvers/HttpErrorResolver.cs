namespace Alpha.NetThrottling.Resolvers
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class HttpErrorResolver : IResolver
    {
        public void Block(HttpContext context)
        {
            throw new HttpException(HttpStatusCodes.TooManyRequests, Lang.BlockedContent);
        }
    }
}
