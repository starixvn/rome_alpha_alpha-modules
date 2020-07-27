namespace Alpha.NetThrottling.Resolvers
{
    using System.Web;

    public class RedirectErrorPageResolver : IResolver
    {
        public void Block(HttpContext context)
        {
            var blockedUrl = $"~/blocked?path={context.Request.Path}";
            context.Response.Redirect(blockedUrl);
        }
    }
}
