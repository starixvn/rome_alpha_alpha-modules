namespace Alpha.Auth.HttpModules
{
    using System;
    using System.Web;
    using System.Web.Security;

    public class AuthenticationHttpModule : IHttpModule
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
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
            }
        }

        #endregion IHttpModule Members
    }
}