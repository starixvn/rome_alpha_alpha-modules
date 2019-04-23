namespace Alpha.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Helpers;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AfterLoginFlowFilter : ActionFilterAttribute
    {
        private readonly IUserSession userSession = new UserSessionExt();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (IsValidSession(filterContext))
            {
                var redirectUrl = GetAfterLoginFlowUrl(filterContext);

                if (redirectUrl != null)
                {
                    filterContext.Result = new RedirectResult(redirectUrl);
                }
            }
        }

        private bool IsValidSession(ActionExecutingContext filterContext)
            => filterContext.HttpContext?.Session != null
            && userSession.UserId > 0
            && !userSession.IsInternal;

        private string GetAfterLoginFlowUrl(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var currentUrl = $"{controllerName}/{actionDescriptor.ActionName}";

            if (IsForceChangePassword)
            {
                return ForceChangePasswordUrl(currentUrl);
            }

            if (userSession.IsForcedSecurityCode)
            {
                return ForceChangeSecurityCodeUrl(currentUrl);
            }

            if (IsSetNickName)
            {
                return CreateNickNameUrl(currentUrl, userSession.CustId);
            }

            return null;
        }

        private bool IsSetNickName
            => !userSession.IsSubAcc && !userSession.IsNicknameLoginOnly;

        private bool IsForceChangePassword
            => !userSession.IsLogged || userSession.IsPasswordExpired;

        private static string CreateNickNameUrl(string currentUrl, int custId)
            => currentUrl.Equals("LoginOption/Index", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.OldMainUrl("~/LoginOption/Index?custid=" + custId);

        private static string ForceChangePasswordUrl(string currentUrl)
            => currentUrl.Equals("Password/ForceChangePassword", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.MainUrl("~/Password/ForceChangePassword");

        private static string ForceChangeSecurityCodeUrl(string currentUrl)
        {
            return currentUrl.Equals("SecurityCode/Index", StringComparison.OrdinalIgnoreCase)
                ? null
                : MicroSites.MainUrl("~/SecurityCode/Index");
        }
    }
}