namespace Alpha.Auth.Filters
{
    using System;
    using System.Configuration;
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
        private bool isAuthorizedChecking;

        public CheckBelongingFilter(bool isAuthorizedChecking = true)
        {
            this.isAuthorizedChecking = isAuthorizedChecking;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated
                && !isAuthorizedChecking)
            {
                isAuthorizedChecking = true;
            }

            if (!isAuthorizedChecking)
            {
                return;
            }

            if (filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.EndsWith("/Auth"))
            {
                return;
            }

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
            var isStandaloneReport = ConfigurationManager.AppSettings["IsStandaloneReport"] == "1";
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
                    var mainDomain = UserSession.Current.GetProperty<string>("MainDomain");
                    var cookieless = filterContext.RequestContext.HttpContext.Session.IsCookieless ?
                        $"/(S({filterContext.RequestContext.HttpContext.Session.SessionID}))" : string.Empty;

                    filterContext.Result = new RedirectResult($"{mainDomain}/ex-main{cookieless}/Error.aspx");
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
                var isStandaloneReport = ConfigurationManager.AppSettings["IsStandaloneReport"] == "1";
                if (isStandaloneReport)
                {
                    var mainDomain = UserSession.Current.GetProperty<string>("MainDomain");
                    if (!message.Equals("DuplicationLogin", StringComparison.OrdinalIgnoreCase))
                    {
                        filterContext.Result = new RedirectResult(mainDomain + MicroSites.OldMainUrl("~/Error.aspx"));
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult($"{mainDomain}{MicroSites.MainUrl()}{AuthorizationSection.Instance.KickoutUrl.TrimStart('~')}?reason={message}");
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