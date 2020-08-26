namespace Alpha.NetThrottling.Resolvers
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class DefaultResolver : IResolver
    {
        public void Block(HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = HttpStatusCodes.TooManyRequests;
            response.StatusDescription = Lang.BlockedContent;
            response.Write(Lang.BlockedContent);
            response.End();
        }
    }
}
