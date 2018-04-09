namespace Alpha.Auth.Filters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
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
                if (!authorizationService.IsAccountActive())
                {
                    KickoutUser(filterContext, "AccountClosed");
                    return;
                }

                if (!AppSettings.IsDisabledCheckDuplicationLogin
                    && authorizationService.IsKickedOut())
                {
                    KickoutUser(filterContext, "DuplicationLogin");
                }
            }
        }

        private void UnAuthorization(AuthorizationContext filterContext)
        {
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
                filterContext.Result = new RedirectResult(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));
            }

            var session = filterContext.HttpContext.Session;
            session.Clear();
            session.Abandon();
            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
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
                filterContext.Result = new RedirectResult(MicroSites.MainUrl($"{AuthorizationSection.Instance.KickoutUrl}?reason={message}"));
            }
        }
    }
}