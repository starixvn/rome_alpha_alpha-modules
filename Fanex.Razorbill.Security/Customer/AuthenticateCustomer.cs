namespace Fanex.Razorbill.Security.Customer
{
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Common;
    using Fanex.CustomerSetting.Security.Authenticate;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Fanex.Razorbill.Security.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticateCustomer : IAuthenticate
    {
        private IUserSession _currentUserSession;

        public AuthenticateAction.AuthenticationType Policy { get; set; }

        public string Function { get; set; }

        public IUserSession CurrentUserSession
        {
            get
            {
                if (null == HttpContext.Current.Session)
                {
                    return null;
                }

                return _currentUserSession ??
                       (_currentUserSession =
                        UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID));
            }
        }

        /// <summary>
        /// Allows customer the view specific function.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Allow to access or not.</returns>
        public bool AllowRead(string action)
        {
            return IsBelong();
        }

        /// <summary>
        /// Allows customer the update specific function.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Allow to update or not.</returns>
        public bool AllowUpdate(string action)
        {
            bool subAccountPermission = !CurrentUserSession.IsSubAcc || CurrentUserSession.SubPageAccess.UpdateAccount;
            return !CurrentUserSession.IsInternal && subAccountPermission && IsBelong();
        }

        /// <summary>
        /// Allows customer the read specific function.
        /// </summary>
        /// <param name="username">The user-name.</param>
        /// <returns>Allow to read or not.</returns>
        public bool AllowReadDepenOnUsernameBelong(string username)
        {
            return IsBelong();
        }

        /// <summary>
        /// Allows customer the update specific function.
        /// </summary>
        /// <param name="action">The action name.</param>
        /// <param name="username">The user-name.</param>
        /// <returns>Allow to update or not.</returns>
        public bool AllowUpdateDepenOnUsernameBelong(string action, string username)
        {
            bool subAccountPermission = !CurrentUserSession.IsSubAcc || CurrentUserSession.SubPageAccess.UpdateAccount;
            return !CurrentUserSession.IsInternal && subAccountPermission && IsBelong();
        }

        /// <summary>
        /// Determines whether the specified customer identifier is belong.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="getUplineInfo">If set to <c>true</c> [get UPLINE information].</param>
        /// <returns>Customer is updating his DOWNLINE.</returns>
        public bool IsBelong(int custId = 0, string username = "", string groupName = null, bool getUplineInfo = true)
        {
            var targetCustomerId = GetCustomerIdFromRequest();
            if (targetCustomerId == 0)
            {
                return true;
            }

            var isValid = false;
            var authorization = new AuthorizationBO(CurrentUserSession.NetSessionId);
            isValid = authorization.CheckBelonging(targetCustomerId, 0, false);

            return isValid;
        }

        private int GetCustomerIdFromRequest()
        {            
            var custId = 0;
            var trackingValue = string.Empty;
            if (HttpContext.Current.Request.Form["model"] != null)
            {
                trackingValue = HttpContext.Current.Request.Form["model"];
            }
            else
            {
                HttpContext.Current.Request.InputStream.Position = 0;
                using (var reader = new System.IO.StreamReader(HttpContext.Current.Request.InputStream))
                {
                    trackingValue = reader.ReadToEnd();
                }
            }

            var belongingParams = WebAppSettings.BelongingParams;
            var paramArray = belongingParams.Split(',');
            foreach (string paramRequest in paramArray)
            {
                if (string.IsNullOrWhiteSpace(paramRequest))
                {
                    continue;
                }

                var currentParamName = Regex.Replace(paramRequest, @"^\s+", string.Empty);
                if (!string.IsNullOrWhiteSpace(trackingValue))
                {
                    var pattern = "(\"" + currentParamName + "\":)([^\r\n][0-9]+)";
                    Match regexMatch = Regex.Match(trackingValue, pattern, RegexOptions.IgnoreCase);
                    if (regexMatch.Success)
                    {
                        var custidFounded = Regex.Replace(regexMatch.Groups[2].ToString(), @"[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);

                        custId = Converter.ToInt(custidFounded);
                    }

                    if (custId > 0)
                    {
                        break;
                    }
                }

                custId = Converter.ToInt(HttpContext.Current.Request[currentParamName]);
                if (custId > 0)
                {
                    break;
                }

                custId = Converter.ToInt(HttpContext.Current.Request.Form[currentParamName]);
                if (custId > 0)
                {
                    break;
                }                    
            }

            return custId;
        }

        /// <summary>
        /// Allows customer the read specific function.
        /// </summary>
        /// <param name="username">The customer indentify .</param>
        /// <returns>Allow to read or not.</returns>
        public bool AllowReadDepenOnCustIdBelong(int custId)
        {
            var authorization = new AuthorizationBO(CurrentUserSession.NetSessionId);
            return authorization.CheckBelonging(custId, 0, false);
        }

        /// <summary>
        /// Allows customer the update specific function.
        /// </summary>
        /// <param name="action">The action name.</param>
        /// <param name="username">The customer indentify .</param>
        /// <returns>Allow to update or not.</returns>
        public bool AllowUpdateDepenOnCustIdBelong(int custId)
        {
            var authorization = new AuthorizationBO(CurrentUserSession.NetSessionId);
            bool subAccountPermission = !CurrentUserSession.IsSubAcc || CurrentUserSession.SubPageAccess.UpdateAccount;
            return !CurrentUserSession.IsInternal && subAccountPermission && authorization.CheckBelonging(custId, 0, false);
        }
    }
}