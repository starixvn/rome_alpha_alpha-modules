namespace Alpha.Auth.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Alpha.Auth.Constants;
    using Configurations;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    /// <summary>
    /// Ricky: should review more in here, we temporary keep this code.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class CheckBelongingFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var authorizationService = new AuthorizationService();
            if (AuthorizationSection.Instance.EnableCheckBelonging)
            {
                if (!authorizationService.IsBelonging())
                {
                    UnAuthorization(filterContext);
                    return;
                }

                if (!authorizationService.IsAuthorized())
                {
                    KickoutUser(filterContext, "NoPermission");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(AuthorizationSection.Instance.KickoutUrl))
            {
                var accountStatus = authorizationService.GetAccountStatus();

                if (accountStatus != null && (accountStatus.IsClosed || accountStatus.IsLocked))
                {
                    KickoutUser(filterContext, accountStatus.IsClosed ? "AccountClosed" : "AccountLocked");
                    return;
                }

                if (!AppSettings.IsDisabledCheckDuplicationLogin)
                {
                    var kickOutInfo = authorizationService.IsKickedOut();

                    if (kickOutInfo.IsKickedOut)
                    {
                        KickoutUser(filterContext, GetKickoutReasonMessage(kickOutInfo.KickOutReason));
                    }
                }
            }
        }

        private void UnAuthorization(AuthorizationContext filterContext)
        {
            var isStandaloneReport = !string.IsNullOrWhiteSpace(UserSession.Current.GetProperty<string>("MainDomain"));
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write("{ \"message\": \"You are not authorized on this function!\" }");
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                if (!isStandaloneReport)
                {
                    filterContext.Result = new RedirectResult(filterContext.HttpContext.Response.ApplyAppPathModifier(FormsAuthentication.LoginUrl));
                }
                else
                {
                    filterContext.Result = new EmptyResult();
                }
            }

            if (!isStandaloneReport)
            {
                var session = filterContext.HttpContext.Session;
                session.Clear();
                session.Abandon();
                if (!session.IsCookieless)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }

        private void KickoutUser(AuthorizationContext filterContext, string message)
        {
            var underMaintenancePageAbsoluteUrl = VirtualPathUtility.ToAbsolute(AuthorizationSection.Instance.KickoutUrl).ToLower();
            var currentPageAbsoluteUrl = filterContext.HttpContext.Request.Url.AbsolutePath.ToLower();
            if (underMaintenancePageAbsoluteUrl == currentPageAbsoluteUrl)
            {
                return;
            }

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                response.Clear();
                response.Write("{ \"message\": \"" + message + "\" }");
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                var isStandaloneReport = !string.IsNullOrWhiteSpace(UserSession.Current.GetProperty<string>("MainDomain"));

                if (isStandaloneReport)
                {
                    if (!message.Equals("DuplicationLogin", StringComparison.OrdinalIgnoreCase))
                    {
                        filterContext.Result = new EmptyResult();
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult(MicroSites.MainUrl($"{AuthorizationSection.Instance.KickoutUrl}?reason={message}"));
                }
            }
        }

        private static string GetKickoutReasonMessage(byte kickOutReason)
        {
            switch (kickOutReason)
            {
                case KickOutReasonConst.ResetPasswordForceKickout:
                    return "KickoutByForceChangePassword";

                case KickOutReasonConst.ProtectedModeForceKickout:
                    return "KickoutByProtectedMode";

                default:
                    return "DuplicationLogin";
            }
        }
    }
}