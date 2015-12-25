namespace Alpha.SecurityCode.HttpModules
{
    using System;
    using System.Web;
    using Services;

    public class SecurityCodeHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(PreRequestHandlerExecute);
        }

        private void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            string redirectUrl = SecurityCodeChecker.GetRedirectUrl();

            if (redirectUrl != null)
            {
                var response = HttpContext.Current.Response;

                response.ContentType = "text/html";
                response.Redirect(redirectUrl, true);
            }
        }

        #endregion IHttpModule Members
    }
}