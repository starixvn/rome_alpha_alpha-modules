namespace Alpha.SecurityCode.Filters
{
    using System;
    using System.Web.Mvc;
    using Alpha.SecurityCode.Services;
    using Alpha.SecurityCode.Web;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class SecurityCodeFilter : ActionFilterAttribute
    {
        private readonly bool isBetLists;
        private readonly bool isStatement;
        private readonly SecurityCodeChecker securityCodeChecker;
        private readonly SecurityCodeHttpContext securityCodeHttpContext;

        public SecurityCodeFilter(bool isBetLists = false, bool isStatement = false)
            : this(isBetLists, isStatement, null, null)
        {
        }

        internal SecurityCodeFilter(
            bool isBetLists,
            bool isStatement,
            SecurityCodeChecker securityCodeChecker,
            SecurityCodeHttpContext securityCodeHttpContext)
        {
            this.isBetLists = isBetLists;
            this.isStatement = isStatement;
            this.securityCodeChecker = securityCodeChecker ?? new SecurityCodeCheckerImpl();
            this.securityCodeHttpContext = securityCodeHttpContext ?? new SecurityCodeHttpContextImpl();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!securityCodeHttpContext.IsValidRequest() || filterContext.IsChildAction)
            {
                return;
            }

            var isRequiredSecurityCode = securityCodeChecker.IsRequiredSecurityCode(isBetLists, isStatement);

            if (isRequiredSecurityCode)
            {
                if (securityCodeHttpContext.IsAjaxRequest())
                {
                    securityCodeHttpContext.DenyAjaxRequest();
                }
                else
                {
                    securityCodeHttpContext.RedirectToVerifySecurityCodePage();
                }
            }
        }
    }
}
