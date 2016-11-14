namespace Alpha.Auth.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Services;
    using Services.Impl;
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
            var authorizationService =  new AuthorizationService();
            if (!authorizationService.IsBelonging())
            {
                UnAuthorization(filterContext);
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
    }
}