namespace Alpha.Auth.Services.Impl
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Alpha.Auth.DataAccesses;
    using Alpha.Auth.DataAccesses.Impl;
    using Alpha.Auth.Services;
    using Entities;
    using Fanex.Common;
    using Fanex.Razorbill.Security.Configurations.AuthenticationSection;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private const string CustomerClosedStatusSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingClosedStatus";
        private const string SubAccountPermissionSessionKey = "Fanex.Razorbill.Security.Services.Authentication.CheckingCustomerPermission";
        private const string IndexPage = "~/Index.aspx";
        private const string SignOutPage = "~/SignOut.aspx";
        private const string AccessDeniedPage = "~/AccessDenied.aspx";

        private readonly IUserSession _currentUserSession;
        private readonly IAuthenticationDataAccess _authenticationRepsitory;
        private readonly IStatisticService _statisticService;

        private string _urlRequest;
        private object _blockStatus = new object();

        public AuthenticationService(string urlRequest)
        {
            _currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            _authenticationRepsitory = new AuthenticationDataAccess();
            _urlRequest = urlRequest;
            _statisticService = new StatisticService();
        }

        public string CustomerCurrentStatus
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, CustomerClosedStatusSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, CustomerClosedStatusSessionKey)] = value;
            }
        }

        public string CustomerCurrentPermission
        {
            get
            {
                return (string)HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, SubAccountPermissionSessionKey)];
            }

            set
            {
                HttpContext.Current.Session[string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, SubAccountPermissionSessionKey)] = value;
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
        public PermissionResult CheckingCustomerPermission(int custId, long transId, bool isDataWarehouse)
        {
            var customerPermission = new PermissionResult()
            {
                IsHasPermission = false,
                RedirectUrl = IndexPage
            };

            // session is null
            if (HttpContext.Current.Session == null)
            {
                customerPermission.RedirectUrl = SignOutPage;
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
            var virtualDirectory = AuthenticationSection.Instance.VirtualDirectory;
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

                                    if (pageElement.IsViewedByGroupHeadline
                                        && _authenticationRepsitory.Age_Rpt_CustomerConsolidatedGroup_CheckBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal))
                                    {
                                        return true;
                                    }

                                    return isDataWarehouse
                                        ? _authenticationRepsitory.Age_CustomerSelCheckBelonging_OLD(custId, _currentUserSession.CustId, transId)
                                        : _authenticationRepsitory.Age_CustomerSelCheckBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal);
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
        private PermissionResult CheckingPageAccess(
            string customerPermission,
            AuthenticationSection authenticationSection,
            int custId,
            long transId,
            bool isDataWarehouse)
        {
            var permissionResult = new PermissionResult()
            {
                IsHasPermission = true
            };

            var levels = from q in authenticationSection.GroupLevel.Cast<GroupLevelElement>() where q.RoleId <= _currentUserSession.RoleId select q;
            if (levels.Any())
            {
                var urlWithOutVirtualDirectory = !string.IsNullOrWhiteSpace(AuthenticationSection.Instance.VirtualDirectory) ?
                    _urlRequest.ToUpper().Replace(string.Format("{0}/", AuthenticationSection.Instance.VirtualDirectory).ToUpper(), string.Empty) :
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
                                        permissionResult.RedirectUrl = AccessDeniedPage;
                                        continue;
                                    }

                                    // Check belonging
                                    if (custId != 0 &&
                                        pageElement.IsCheckBelonging &&
                                        !(pageElement.IsAllowToDoByThemselves && UserSession.Current.CustId == custId))
                                    {
                                        var hasPermission = isDataWarehouse
                                            ? (new AuthorizationDataAccess()).Age_CustomerSelCheckBelonging_OLD(custId, _currentUserSession.CustId, transId)
                                            : (new AuthorizationDataAccess()).Age_CustomerSelCheckBelonging(custId, _currentUserSession.CustId, _currentUserSession.SubAccId, transId, _currentUserSession.IsInternal);
                                        if (!hasPermission)
                                        {
                                            permissionResult.IsHasPermission = false;
                                            permissionResult.RedirectUrl = AccessDeniedPage;
                                        }
                                    }

                                    if (!_currentUserSession.IsInternal && permissionResult.IsHasPermission)
                                    {
                                        _statisticService.UpdateFunctionUsage(pageElement.Url, pageElement.MainFunctionName, pageElement.SubFunctionName, _currentUserSession.Site);
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