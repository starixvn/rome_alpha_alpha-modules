namespace Alpha.Auth.Web.Validators
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Helpers;
    using Configurations;
    using Models;

    /// <summary>
    /// http://blog.stevensanderson.com/2008/09/01/prevent-cross-site-request-forgery-csrf-using-aspnet-mvcs-antiforgerytoken-helper/
    /// </summary>
    public static class AntiForgeryValidator
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";

        public static Result ValidateRequest()
        {
            if (!AlphaAuthSection.Instance.EnableAntiXSRF)
            {
                return Result.Success;
            }

            try
            {
                var request = HttpContext.Current.Request;

                var formToken = request.Headers[AntiForgeryFormTokenName];
                if (string.IsNullOrWhiteSpace(formToken))
                {
                    ValidationLogger.WriteWarning("Request Verification Token in Form does not exist.");

                    return Result.GetAccessDeniedResult("Request Verification Token does not exist. Please refresh your page. Thanks.");
                }

                var cookieValue = request.Cookies[AntiForgeryConfig.CookieName];
                if (cookieValue != null
                    && !string.IsNullOrWhiteSpace(cookieValue.Value))
                {
                    AntiForgery.Validate(cookieValue.Value, formToken);
                }
            }
            catch (Exception ex)
            {
                ValidationLogger.WriteWarning(ex, "Validation of Anti-XSRF token failed");

                return Result.GetAccessDeniedResult("Validation of Anti-XSRF token failed. Please refresh your page. Thanks.");
            }

            return Result.Success;
        }

        public static string GenerateTokenInInputElement()
        {
            return AntiForgery.GetHtml().ToHtmlString();
        }
    }
}