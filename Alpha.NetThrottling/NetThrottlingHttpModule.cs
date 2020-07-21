namespace Alpha.NetThrottling
{
    using Alpha.NetThrottling.Utils;
    using System;
    using System.Threading.Tasks;
    using System.Web;

    public class NetThrottlingHttpModule : IHttpModule
    {
        private NetThrottler netThrottler;

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        public void Init(HttpApplication context)
        {
            netThrottler = new NetThrottler();
            var wrapper = new EventHandlerTaskAsyncHelper(ThrottleRequest);
            context.AddOnBeginRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
        }

        async Task ThrottleRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var allowed = await netThrottler.ThrottleAsync(context);
            if (!allowed)
            {
                var message = "Too many requests";
                context.Response.StatusCode = HttpStatusCodes.TooManyRequests;
                context.Response.StatusDescription = message;
                context.Response.Write(message);
                context.Response.End();
            }
        }
    }
}
