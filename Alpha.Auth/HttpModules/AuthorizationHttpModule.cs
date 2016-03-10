namespace Alpha.Auth.HttpModules
{
    using System;
    using System.Web;
    using System.Web.Security;
    using Services;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizationHttpModule : IHttpModule
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
            //if (string.IsNullOrEmpty(url) ||
            //    url.Replace("/", string.Empty).Equals(AuthorizationSection.Instance.VirtualDirectory, StringComparison.OrdinalIgnoreCase))
            //{
            //    HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl, true);
            //    return;
            //}

            IAuthorizationService authorizationService = new AuthorizationService();
            if (UserSession.Current.CustId != 0 && !UserSession.Current.IsInternal)
            {
                if (authorizationService.CheckClosedStatus())
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
                    return;
                }
            }

            var isBelonging = authorizationService.CheckBelonging();
            if (!isBelonging)
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
                return;
            }

            var permissionResult = authorizationService.CheckPermission();
            if (!permissionResult.HasPermission)
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Redirect("unauthorized", true);
            }
        }

        #endregion IHttpModule Members
    }
}