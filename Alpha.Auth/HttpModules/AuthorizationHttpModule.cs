namespace Alpha.Auth.HttpModules
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using System.Web.SessionState;
    using Configurations;
    using Services;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;

    public class AuthorizationHttpModule : IHttpModule, IRequiresSessionState
    {
        public bool IsIgnore
        {
            get
            {
                var pattern = "([^\\s]+(\\.(?i)(jpg|png|gif|bmp|css|js))$)";

                Regex urlRegex = new Regex(pattern);

                return urlRegex.IsMatch(HttpContext.Current.Request.Path, (int)RegexOptions.IgnoreCase);
            }
        }

        public bool IsSessionNull
        {
            get
            {
                return HttpContext.Current.Session == null;
            }
        }

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
            string executeFileName = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            var ignoreUrls = AuthorizationSection.Instance.IgnoreUrls.Split(',');

            foreach (var ignoreUrl in ignoreUrls)
            {
                if (executeFileName.EndsWith(ignoreUrl, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            if (executeFileName.EndsWith("SignIn.aspx")
               || executeFileName.EndsWith("ProcessAdminLogin.ashx")
               || executeFileName.EndsWith("ProcessAfterLogin.ashx")
               || executeFileName.EndsWith("Captcha.ashx")
               || executeFileName.EndsWith("ChartImg.axd")
               || executeFileName.EndsWith("ReCheckin.ashx")
               || executeFileName.EndsWith("SecurityCode")
               || executeFileName.EndsWith("Header.aspx"))
            {
                return;
            }

            if (IsSessionNull || IsIgnore)
            {
                return;
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated || UserSession.Current.CustId == 0)
            {
                HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
            }

            IAuthorizationService authorizationService = new AuthorizationService();
            if (UserSession.Current.CustId != 0 && !UserSession.Current.IsInternal && !authorizationService.IsAccountActive())
            {
                GoToLoginPage();

                return;
            }

            var isBelonging = authorizationService.CheckBelonging();
            if (!isBelonging)
            {
                GoToLoginPage();

                return;
            }
        }

        //// Permission for sub account still have not implemented yet.
        ////var permissionResult = authorizationService.CheckPermission();
        ////if (!permissionResult.HasPermission)
        ////{
        ////    HttpContext.Current.Session.Abandon();
        ////    HttpContext.Current.Response.Redirect("unauthorized", true);
        ////}

        private void GoToLoginPage()
        {
            var session = HttpContext.Current.Session;

            session.Clear();
            session.Abandon();
            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }

            HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl, true);
        }

        #endregion IHttpModule Members
    }
}