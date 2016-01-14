namespace Alpha.Auth.HttpModules
{
    using System;
    using System.Web;
    using System.Web.Security;
    using Sunplus.Agent.Model.Entities;

    public class AuthenticationHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(AcquireRequestState);
        }

        private void AcquireRequestState(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated
                || UserSession.Current.CustId == 0)
            {
                HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
            }
        }

        #endregion IHttpModule Members
    }
}