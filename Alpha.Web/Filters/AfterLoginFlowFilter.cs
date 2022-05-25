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
        private bool isAuthorizedChecking;

        public AfterLoginFlowFilter(bool isAuthorizedChecking = true)
        {
            this.isAuthorizedChecking = isAuthorizedChecking;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
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
            && !filterContext.IsChildAction
            && userSession.UserId > 0
            && !userSession.IsInternal;

        private string GetAfterLoginFlowUrl(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var currentUrl = $"{controllerName}/{actionDescriptor.ActionName}";

            if (!IsInIgnoreUrl(currentUrl))
            {
                if (IsForceChangePassword)
                {
                    return ForceChangePasswordUrl(currentUrl);
                }

                if (userSession.IsForcedSecurityCode)
                {
                    return ForceChangeSecurityCodeUrl(currentUrl, userSession.SecurityCode);
                }

                if (IsForceLoginByNickname)
                {
                    return CreateNickNameUrl(currentUrl);
                }
            }

            return null;
        }

        private bool IsInIgnoreUrl(string currentUrl)
            => currentUrl.Equals("Language/Change", StringComparison.OrdinalIgnoreCase)
            || currentUrl.Equals("Default/Kickout", StringComparison.OrdinalIgnoreCase);

        private bool IsForceChangePassword
            => !userSession.IsLogged || userSession.IsPasswordExpired;

        private static string ForceChangePasswordUrl(string currentUrl)
            => currentUrl.Equals("Password/ForceChangePassword", StringComparison.OrdinalIgnoreCase) ||
            currentUrl.Equals("Password/SkipForceChangePassword", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.MainUrl("~/Password/ForceChangePassword");

        private static string ForceChangeSecurityCodeUrl(string currentUrl, string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                return currentUrl.Equals("SecurityCode/ChangeSecurityCodeFirstLogin", StringComparison.OrdinalIgnoreCase)
                    ? null
                    : "~/SecurityCode/ChangeSecurityCodeFirstLogin";
            }

            return currentUrl.Equals("SecurityCode/ChangeSecurityCodeByForcing", StringComparison.OrdinalIgnoreCase)
                ? null
                : "~/SecurityCode/ChangeSecurityCodeByForcing";
        }

        private bool IsForceLoginByNickname
        {
            get
            {
                NicknameChecker nicknameChecker = new NicknameCheckerImpl(userSession: userSession);

                return nicknameChecker.IsForceLoginByNickname;
            }
        }

        private static string CreateNickNameUrl(string currentUrl)
            => currentUrl.Equals("Nickname/Index", StringComparison.OrdinalIgnoreCase)
            ? null
            : MicroSites.MainUrl("~/Nickname/Index");
    }
}