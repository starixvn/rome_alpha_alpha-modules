namespace Fanex.Razorbill.Security.Services.Authentication
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Common;
    //using Fanex.Razorbill.Module.Common.Configurations;
    using Fanex.Razorbill.Security.Configurations.AuthenticationSection;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Repositories.Authentication;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Fanex.Razorbill.Security.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private string _customerClosedStatusSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingClosedStatus";
        private string _subAccountPermissionSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingCustomerPermission";
        private IUserSession _currentUserSession;
        private IAuthenticationRepository _authenticationRepsitory;
        private IUserSession _userSession;

        private string _urlRequest;
        private object _blockStatus = new object();
        private string _indexPage = "~/Index.aspx";
        private string _signOutPage = "~/SignOut.aspx";
        private string _accessDeniedPage = "~/AccessDenied.aspx";

        public AuthenticationService(string urlRequest)
        {
            _currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            _authenticationRepsitory = new AuthenticationRepository();
            _urlRequest = urlRequest;
        }

        public string CustomerCurrentStatus
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, _customerClosedStatusSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, _customerClosedStatusSessionKey)] = value;
            }
        }

        public string CustomerCurrentPermission
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, _subAccountPermissionSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, _subAccountPermissionSessionKey)] = value;
            }
        }

        /// <summary>
        /// Checking customer status.
        /// </summary>
        /// <returns>Return result.</returns>
        public bool CheckingClosedStatus()
        {
            lock (_blockStatus)
            {
                if (HttpContext.Current.Session == null)
                {
                    return true;
                }

                if (IgnoreSpecialFileTypes())
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(CustomerCurrentStatus))
                {
                    var sessionInfo = CustomerCurrentStatus.Split('_');
                    if (sessionInfo.Length > 1 &&
                        Converter.ToDateTime(sessionInfo[1]) >= DateTime.Now)
                    {
                        return false;
                    }
                }

                var isActive = _authenticationRepsitory.CheckingCustomerStatus(_currentUserSession.CustId, _currentUserSession.SubAccId);
                if (!isActive)
                {
                    return true;
                }

                CustomerCurrentStatus =
                    string.Format("{0}_{1}", isActive, DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"])));

                return false;
            }
        }

        /// <summary>
        /// Function to check belonging.
        /// </summary>
        /// <param name="custId">The Customer ID.</param>
        /// <param name="transId">Transaction ID.</param>
        /// <param name="isDataWarehouse">Is Data warehouse.</param>
        /// <returns>Return belonging status.</returns>
        public bool CheckBelonging(int custId, long transId, bool isDataWarehouse)
        {
            if (HttpContext.Current.Session == null)
            {
                return false;
            }

            var currentUrlAccess = GetUrlAccessByCustomerWithoutDirectoryPath();

            if (IgnoreSpecialFileTypes() ||
                currentUrlAccess.Equals("index.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var isBelonging = VerifyRequestingUrlAgainstConfigFile(custId, transId, isDataWarehouse, currentUrlAccess);

            return isBelonging;
        }

        /// <summary>
        /// Checking Customer's Permission.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="transId">Transaction ID.</param>
        /// <param name="isDataWarehouse">Is Data Warehouse.</param>
        /// <returns>Return result.</returns>
        public CustomerPermissionResult CheckingCustomerPermission(int custId, long transId, bool isDataWarehouse)
        {
            var customerPermission = new CustomerPermissionResult()
            {
                IsHasPermission = false,
                RedirectUrl = _indexPage
            };

            // session is null
            if (HttpContext.Current.Session == null)
            {
                customerPermission.RedirectUrl = _signOutPage;
                return customerPermission;
            }

            // ignore urls
            if (IgnoreSpecialFileTypes())
            {
                customerPermission.IsHasPermission = true;
                return customerPermission;
            }

            var authenticationSection = AuthenticationSection.Instance;
            var permission = authenticationSection.FullPermission;
            if (_currentUserSession.SubAccId > 0)
            {
                permission = GetSubPermission();
                if (permission.Equals("0"))
                {
                    return customerPermission;
                }
            }

            var newCustomerPermission = CheckingPageAccess(permission, authenticationSection, custId, transId, isDataWarehouse);
            customerPermission.IsHasPermission = newCustomerPermission.IsHasPermission;
            customerPermission.RedirectUrl = newCustomerPermission.RedirectUrl;

            return customerPermission;
        }

        private bool IgnoreSpecialFileTypes()
        {
            var pattern = "([^\\s]+(\\.(?i)(jpg|png|gif|bmp|css|js))$)";

            Regex urlRegex = new Regex(pattern);

            return urlRegex.IsMatch(_urlRequest, (int)RegexOptions.IgnoreCase);
        }

        private string GetUrlAccessByCustomerWithoutDirectoryPath()
        {
            var virtualDirectory = WebAppSettings.VirtualDirectory;
            if (!string.IsNullOrWhiteSpace(virtualDirectory))
            {
                var regex = new Regex(string.Format("{0}/", virtualDirectory), RegexOptions.IgnoreCase);
                var urlWithOutVirtualDirectory = regex.Replace(_urlRequest, string.Empty);

                return urlWithOutVirtualDirectory;
            }

            return _urlRequest;
        }

        private bool VerifyRequestingUrlAgainstConfigFile(int custId, long transId, bool isDataWarehouse, string currentUrlAccess)
        {
            var isBelonging = true;
            var authenticationSection = AuthenticationSection.Instance;
            var permission = authenticationSection.FullPermission;

            // get all urls that allow he/she access by his/her level
            var levels = from q in authenticationSection.GroupLevel.Cast<GroupLevelElement>() where q.RoleId <= _currentUserSession.RoleId select q;
            if (levels.Any())
            {
                foreach (GroupLevelElement level in levels)
                {
                    // get setting of current url against config file
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = from q in level.GroupLevel.Cast<PageAccessElement>()
                                         where q.Url.Equals(currentUrlAccess, StringComparison.OrdinalIgnoreCase)
                                         select q;

                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                // Check belonging when:
                                // - downline id not equal 0
                                // - and current requesting url need to check belonging
                                // - and current requesting url not in special list: allow customers to update by themselves
                                if (custId != 0 && pageElement.IsCheckBelonging)
                                {
                                    if (!pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId)
                                    {
                                        return false;
                                    }

                                    if (pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId)
                                    {
                                        return true;
                                    }

                                    var netSessionId = HttpContext.Current.Session.SessionID;
                                    _userSession = UserSessionService.GetUserSessionInstance(netSessionId);

                                    if (pageElement.IsViewedByGroupHeadline 
                                        && _authenticationRepsitory.Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(custId, _userSession.CustId, _userSession.SubAccId, transId, _userSession.IsInternal))
                                    {
                                        return true;
                                    }

                                    return isDataWarehouse
                                        ? _authenticationRepsitory.Age_CustomerSelCheckBelonging_OLD(custId, _userSession.CustId, transId)
                                        : _authenticationRepsitory.Age_CustomerSelCheckBelonging(custId, _userSession.CustId, _userSession.SubAccId, transId, _userSession.IsInternal);
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return isBelonging;
        }

        /// <summary>
        /// Page access authentication.
        /// </summary>
        /// <param name="customerPermission">Customer's Permission.</param>
        /// <param name="authenticationSection">Authentication Section.</param>
        /// <param name="custId">Customer ID.</param>
        /// <param name="transId">Transaction ID.</param>
        /// <param name="isDataWarehouse">IS Data Warehouse.</param>
        /// <returns>Return result.</returns>
        private CustomerPermissionResult CheckingPageAccess(
            string customerPermission,
            AuthenticationSection authenticationSection,
            int custId,
            long transId,
            bool isDataWarehouse)
        {
            var permissionResult = new CustomerPermissionResult()
            {
                IsHasPermission = true
            };

            var levels = from q in authenticationSection.GroupLevel.Cast<GroupLevelElement>() where q.RoleId <= _currentUserSession.RoleId select q;
            if (levels.Any())
            {
                var urlWithOutVirtualDirectory = !string.IsNullOrWhiteSpace(WebAppSettings.VirtualDirectory) ?
                    _urlRequest.ToUpper().Replace(string.Format("{0}/", WebAppSettings.VirtualDirectory).ToUpper(), string.Empty) :
                    string.Empty;

                foreach (GroupLevelElement level in levels)
                {
                    if (level != null && level.GroupLevel != null && level.GroupLevel.Count > 0)
                    {
                        var pageAccess = from q in level.GroupLevel.Cast<PageAccessElement>()
                                         where q.Url.Equals(urlWithOutVirtualDirectory, StringComparison.OrdinalIgnoreCase)
                                         select q;

                        if (pageAccess.Any())
                        {
                            foreach (PageAccessElement pageElement in pageAccess)
                            {
                                var requiredPermission = pageElement.Permission.Split(',');
                                foreach (var item in requiredPermission)
                                {
                                    if (!customerPermission.Contains(item))
                                    {
                                        permissionResult.IsHasPermission = false;
                                        permissionResult.RedirectUrl = _accessDeniedPage;
                                        continue;
                                    }

                                    // Check belonging
                                    if (custId != 0 &&
                                        pageElement.IsCheckBelonging &&
                                        !(pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId))
                                    {
                                        AuthorizationBO authorization = new AuthorizationBO(HttpContext.Current.Session.SessionID);
                                        if (!authorization.CheckBelonging(custId, transId, isDataWarehouse))
                                        {
                                            permissionResult.IsHasPermission = false;
                                            permissionResult.RedirectUrl = _accessDeniedPage;
                                        }
                                    }

                                    if (!_currentUserSession.IsInternal && permissionResult.IsHasPermission)
                                    {
                                        var statistic = new Sunplus.Agent.Model.BL.Statistic.StatisticBO(HttpContext.Current.Session.SessionID);
                                        statistic.UpdateFunctionUsage(pageElement.Url, pageElement.MainFunctionName, pageElement.SubFunctionName, _currentUserSession.Site);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }

            return permissionResult;
        }

        /// <summary>
        /// Get Sub Account permission.
        /// </summary>
        /// <returns>Return result.</returns>
        private string GetSubPermission()
        {
            if (!string.IsNullOrEmpty(CustomerCurrentPermission))
            {
                var sessionInfo = CustomerCurrentPermission.Split('_');
                if (sessionInfo.Length > 1 &&
                    Converter.ToDateTime(sessionInfo[1]) >= DateTime.Now)
                {
                    return Converter.ToString2(sessionInfo[0]);
                }
            }

            var newPermission = _authenticationRepsitory.GetSubAccountPermission(_currentUserSession.SubAccId);
            CustomerCurrentPermission =
                string.Format("{0}_{1}", newPermission, DateTime.Now.AddSeconds(Converter.ToInt(ConfigurationManager.AppSettings["CustomerStatusExpireTime"])));

            return newPermission;
        }
    }
}