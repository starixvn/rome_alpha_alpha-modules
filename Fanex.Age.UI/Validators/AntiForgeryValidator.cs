namespace Fanex.Age.UI.Validators
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Helpers;

    /// <summary>
    /// http://blog.stevensanderson.com/2008/09/01/prevent-cross-site-request-forgery-csrf-using-aspnet-mvcs-antiforgerytoken-helper/
    /// </summary>
    public static class AntiForgeryValidator
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";
        private static readonly bool _enableAntiXSRF = true;

        static AntiForgeryValidator()
        {
            var enableAntiXSRF = ConfigurationManager.AppSettings["EnableAntiXSRF"];
            if (!string.IsNullOrWhiteSpace(enableAntiXSRF))
            {
                _enableAntiXSRF = enableAntiXSRF != "0";
            }
        }

        public static Result ValidateRequest()
        {
            if (!_enableAntiXSRF)
            {
                return new Result(ErrorType.Success, string.Empty);
            }

            try
            {
                var request = HttpContext.Current.Request;

                var formToken = request.Headers[AntiForgeryFormTokenName];
                if (!string.IsNullOrWhiteSpace(formToken))
                {
                    return new Result(ErrorType.AccessDenied, "Request Verification Token does not exist!");
                }

                var cookieValue = request.Cookies[AntiForgeryConfig.CookieName];
                if (cookieValue != null
                    && !string.IsNullOrWhiteSpace(cookieValue.Value))
                {
                    AntiForgery.Validate(cookieValue.Value, formToken);
                }
                else
                {
                    ValidationLogger.WriteWarning("Cannot read Request Verification Token from cookie.");
                }
            }
            catch (Exception ex)
            {
                ValidationLogger.WriteWarning(ex, "Validation of Anti-XSRF token failed");

                return new Result(ErrorType.AccessDenied, "Validation of Anti-XSRF token failed. Please refresh your page. Thanks.");
            }

            return new Result(ErrorType.Success, string.Empty);
        }
    }
}