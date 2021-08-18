namespace Alpha.Web.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Alpha.Web.Helpers;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ProtectedModeFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                return;
            }

            if (UserSession.Current.IsProtectedMode)
            {
                var actionDescriptor = filterContext.ActionDescriptor;
                var allowProtectionModeAttributeType = typeof(AllowProtectedModeAttribute);

                if (!actionDescriptor.IsDefined(allowProtectionModeAttributeType, true)
                    && !actionDescriptor.ControllerDescriptor.IsDefined(allowProtectionModeAttributeType, true))
                {
                    UnAuthorizedAction(filterContext);
                }
            }
        }

        private static void UnAuthorizedAction(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            if (httpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

                var response = httpContext.Response;
                response.Clear();
                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));
            }

            TerminateSession(httpContext.Session);
        }

        private static void TerminateSession(HttpSessionStateBase session)
        {
            session.Clear();
            session.Abandon();

            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}
