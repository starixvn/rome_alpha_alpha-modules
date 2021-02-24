namespace Alpha.NetThrottling.Resolvers
{
    using Alpha.NetThrottling.Resources;
    using Alpha.NetThrottling.Utils;
    using System.Web;

    public class BaseResolver : IResolver
    {
        public void Block()
        {
            Block(new HttpContextWrapper(HttpContext.Current));
        }

        public virtual void Block(HttpContextBase context)
        {
            if (context.Request.IsAjaxRequest())
            {
                BlockAjaxRequest(context);
            }
            else
            {
                BlockNonAjaxRequest(context);
            }
        }

        public virtual void BlockAjaxRequest(HttpContextBase context)
        {
            BlockRequest(context);
        }

        public virtual void BlockNonAjaxRequest(HttpContextBase context)
        {
            BlockRequest(context);
        }

        private void BlockRequest(HttpContextBase context)
        {
            var response = context.Response;
            response.StatusCode = HttpStatusCodes.TooManyRequests;
            response.StatusDescription = Lang.BlockedContent;
            response.Write(Lang.BlockedContent);
            response.End();
        }
    }
}
