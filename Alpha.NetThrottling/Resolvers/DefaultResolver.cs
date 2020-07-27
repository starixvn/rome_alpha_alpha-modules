namespace Alpha.NetThrottling.Resolvers
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class DefaultResolver : IResolver
    {
        public void Block(HttpContext context)
        {
            context.Response.StatusCode = HttpStatusCodes.TooManyRequests;
            context.Response.StatusDescription = Lang.BlockedContent;
            context.Response.Write(Lang.BlockedContent);
            context.Response.End();
        }
    }
}
