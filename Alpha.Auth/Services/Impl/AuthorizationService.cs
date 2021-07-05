namespace Alpha.Auth.Services.Impl
{
    using System.Linq;
    using Alpha.Auth.DataAccesses;
    using Alpha.Auth.DataAccesses.Impl;
    using Alpha.Auth.Services;
    using Configurations;
    using Models;

    /// <summary>
    /// Permission Description
    /// [A]: Create
    /// [B]: Update
    /// [C]: Transfer
    /// [D]: Total Bets
    /// [E]: Reports
    /// [F]: Forcast
    /// [H]: Member Information
    /// [G]: View Log
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private const string AccountStatusCacheKey = "Alpha.Auth.CustContext.AccountStatusCacheKey_{0}_{1}";
        private const string IsCustomerActiveCacheKey = "Alpha.Auth.CustContext.IsCustomerActiveCacheKey_{0}_{1}";
        private const string RecheckInCacheKey = "Alpha.Auth.CustContext.RecheckInCacheKey_{0}_{1}_{2}";
        private readonly int _cachedExpireTime;

        private readonly IAuthorizationDataAccess _authDataAccess;
        private readonly IRequestContext _requestContext;
        private readonly ICachingService _cachingService;
        private readonly AuthorizationSection _authorizationSection;

        public AuthorizationService(
            IRequestContext requestContext = null,
            ICachingService cachingService = null,
            IAuthorizationDataAccess dataAccess = null,
            AuthorizationSection authorizationSection = null)
        {
            _authDataAccess = dataAccess ?? new AuthorizationDataAccess();
            _cachingService = cachingService ?? new CachingService();
            _requestContext = requestContext ?? new RequestContext();
            _authorizationSection = authorizationSection ?? AuthorizationSection.Instance;
            _cachedExpireTime = _authorizationSection.CachedExpireTime;
        }

        public AccountStatus GetAccountStatus()
        {
            if (_requestContext.UserSession.IsInternal)
            {
                return new AccountStatus(true, 0);
            }

            var accountStatus = _cachingService.GetOrAdd(
                string.Format(AccountStatusCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
                () => _authDataAccess.GetAccountStatus(_requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
                _cachedExpireTime);

            return accountStatus;
        }

        public bool IsAccountActive()
        {
            if (_requestContext.UserSession.IsInternal)
            {
                return true;
            }

            var isActive = _cachingService.GetOrAdd(
                string.Format(IsCustomerActiveCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
                () =>
                {
                    return _authDataAccess.IsActive(_requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId);
                },
                _cachedExpireTime);

            return isActive;
        }

        public bool IsBelonging()
        {
            var isBelonging = true;
            var currentUrl = _requestContext.Url.ToUpperInvariant();

            foreach (GroupPageAccessElement groupPageAccess in _authorizationSection.GroupPageAccessList)
            {
                if (groupPageAccess.RoleId <= _requestContext.UserSession.RoleId)
                {
                    foreach (PageAccessElement pageAccess in groupPageAccess.PageAccesses)
                    {
                        if (currentUrl.Contains(pageAccess.Url.ToUpperInvariant())
                            && _requestContext.CustId != 0
                            && pageAccess.IsCheckBelonging)
                        {
                            return IsBelongingOnPage(pageAccess);
                        }
                    }
                }
            }

            return isBelonging;
        }

        private bool IsBelongingOnPage(PageAccessElement pageAccess)
        {
            // Check belonging when:
            // - downline id not equal 0
            // - and current requesting url need to check belonging
            // - and current requesting url not in special list: allow customers to update by themselves
            if (_requestContext.UserSession.CustId == _requestContext.CustId)
            {
                return pageAccess.IsAllowToDoByThemselves;
            }

            if (pageAccess.IsViewedByGroupHeadline
                && _authDataAccess.IsCustomerConsolidatedGroupBelonging(
                    _requestContext.CustId,
                    _requestContext.UserSession.CustId,
                    _requestContext.UserSession.SubAccId,
                    _requestContext.TransId,
                    _requestContext.UserSession.IsInternal))
            {
                return true;
            }

            return _requestContext.IsDataStoreInDataWarehouse
                ? _authDataAccess.IsBelongingOld(
                    _requestContext.CustId,
                    _requestContext.UserSession.CustId,
                    _requestContext.TransId)

                : IsBelonging(_requestContext);
        }

        internal bool IsBelonging(IRequestContext requestContext)
        {
            var belongingInfo = _authDataAccess.IsBelonging(
                    requestContext.CustId,
                    requestContext.UserSession.CustId,
                    requestContext.UserSession.SubAccId,
                    requestContext.TransId);

            if (belongingInfo.RoleId == 0
                || belongingInfo.RoleId >= requestContext.UserSession.RoleId)
            {
                return false;
            }

            if (requestContext.UserSession.IsInternal)
            {
                return true;
            }

            return belongingInfo.IsActive;
        }

        public bool IsAuthorized(string url = null)
        {
            url = url ?? _requestContext.Url;            

            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            url = url.ToUpperInvariant();
            var subAccHasPermissionToViewUrl = true;
            if (_requestContext.UserSession.SubAccId > 0)
            {
                subAccHasPermissionToViewUrl = CheckSubAccHasPermissionToViewUrl(
                    _requestContext.UserSession.SubPageAccess
                    , url);
            }

            return HasPermissionToViewUrl(url) && subAccHasPermissionToViewUrl;
        }

        private bool HasPermissionToViewUrl(string currentUrlAccess)
        {
            var deniedpages = DeniedPageHelper.GetDeniedPages(_requestContext.UserSession.RoleId);

            return !deniedpages.Any(i => currentUrlAccess.Contains(i.Url.ToUpperInvariant()));
        }

        private bool CheckSubAccHasPermissionToViewUrl(
            string permissions,
            string url)
        {
            foreach (var groupPageAccess in _authorizationSection.GroupPageAccessList)
            {
                if (groupPageAccess.RoleId <= _requestContext.UserSession.RoleId)
                {
                    foreach (PageAccessElement pageAccess in groupPageAccess.PageAccesses)
                    {
                        if (url.Contains(pageAccess.Url.ToUpperInvariant()))
                        {
                            var isAuthorized = IsAuthorizedOnPage(permissions, pageAccess);

                            if (!isAuthorized)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool IsAuthorizedOnPage(string permissions, PageAccessElement pageElement)
        {
            var requiredPermission = pageElement.Permission.Split(',');

            foreach (var permission in requiredPermission)
            {
                if (permissions.Contains(permission))
                {
                    return true;
                }
            }

            return false;
        }

        ////private string GetSubPermission()
        ////{
        ////    var subAccountPermission = _cachingService.GetOrAdd(
        ////        string.Format(SubAccountPermissionCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
        ////        () =>
        ////        {
        ////            return _authDataAccess.GetSubAccountPermission(_requestContext.UserSession.SubAccId);
        ////        },
        ////        _cachedExpireTime);

        ////    return subAccountPermission;
        ////}

        public KickoutInfo IsKickedOut()
        {
            if (_requestContext.UserSession.IsInternal)
            {
                return new KickoutInfo(false);
            }

            if (_authorizationSection.IgnoreRecheckinCustIds.Contains(_requestContext.UserSession.CustId))
            {
                return new KickoutInfo(false);
            }

            var onlineUserId = _requestContext.UserSession.OnUserId;

            if (onlineUserId <= 0)
            {
                return new KickoutInfo(true);
            }

            var kickoutInfo = _cachingService.GetOrAdd(
                string.Format(RecheckInCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId, onlineUserId),
                () =>
                {
                    return _authDataAccess.IsKickedOut(onlineUserId);
                },
                _cachedExpireTime);

            return kickoutInfo;
        }
    }
}