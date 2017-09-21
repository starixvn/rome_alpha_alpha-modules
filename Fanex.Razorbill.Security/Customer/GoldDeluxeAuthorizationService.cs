namespace Fanex.Razorbill.Security.Customer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.GoldDeluxe.CoreSetting.Services;

    /// <summary>
    /// Gold Deluxe authentication.
    /// </summary>
    public class GoldDeluxeAuthorizationService : IGoldDeluxeAuthorizationService
    {
        private AuthenticateCustomer _authenticateCustomer = new AuthenticateCustomer();
        private SettingService _livecasinoService = new SettingService();

        public bool IsAuthorizedRead(AuthorizationContext authorizationContext, int custId)
        {
            return _authenticateCustomer.AllowReadDepenOnCustIdBelong(custId);
        }

        public bool IsAuthorizedHost(AuthorizationContext authorizationContext, int custId, int actorId)
        {
            return custId != actorId && _authenticateCustomer.AllowUpdateDepenOnCustIdBelong(custId);
        }

        /// <summary>
        /// Authentication multiple.
        /// </summary>
        /// <param name="authorizationContext">Authorization Context object.</param>
        /// <param name="usernames">User name list.</param>
        /// <returns>Boolean object.</returns>
        public bool IsAuthorizedReadMultiple(AuthorizationContext authorizationContext, IEnumerable<string> usernames)
        {
            foreach (var customer in usernames)
            {
                if (!_authenticateCustomer.AllowReadDepenOnUsernameBelong(customer))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Authentication multiple.
        /// </summary>
        /// <param name="authorizationContext">Authorization Context object.</param>
        /// <param name="usernames">User name list.</param>
        /// <returns>Boolean object.</returns>
        public bool IsAuthorizedHostMultiple(AuthorizationContext authorizationContext, IEnumerable<string> usernames)
        {
            var custIds = GetCustomerIds();
            if (string.IsNullOrWhiteSpace(custIds))
            {
                return false;
            }

            custIds = Regex.Replace(custIds, @"[^a-zA-Z0-9_.]+", ",", RegexOptions.Compiled);
            var custArray = custIds.Split(',');

            foreach (var custId in custArray)
            {
                if (string.IsNullOrWhiteSpace(custId))
                {
                    continue;
                }

                if (!_authenticateCustomer.AllowUpdateDepenOnCustIdBelong(Converter.ToInt(custId)))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetCustomerIds()
        {
            var custIds = string.Empty;
            HttpContext.Current.Request.InputStream.Position = 0;
            var trackingValue = string.Empty;
            using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                trackingValue = reader.ReadToEnd();
            }

            if (!string.IsNullOrWhiteSpace(trackingValue))
            {
                var pattern = "(CustIds\":)([^\r\n]+)(,\"Usernames\")";
                Match regexMatch = Regex.Match(trackingValue, pattern, RegexOptions.IgnoreCase);
                if (regexMatch.Success)
                {
                    custIds = Converter.ToString2(regexMatch.Groups[2]);
                }
            }

            return custIds;
        }
    }
}