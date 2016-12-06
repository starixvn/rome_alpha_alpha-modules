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
        private const string IsCustomerActiveCacheKey = "Alpha.Auth.CustContext.IsCustomerActiveCacheKey_{0}_{1}";
        private const string SubAccountPermissionCacheKey = "Alpha.Auth.CustContext.SubAccountPermissionCacheKey_{0}_{1}";
        private const string RecheckInCacheKey = "Alpha.Auth.CustContext.RecheckInCacheKey_{0}_{1}_{2}";
        private readonly int _cachedExpireTime;

        private readonly IAuthorizationDataAccess _authDataAccess;
        private readonly IStatisticService _statisticService;
        private readonly IRequestContext _requestContext;
        private readonly ICachingService _cachingService;
        private readonly AuthorizationSection _authorizationSection;

        public AuthorizationService(
            IStatisticService statisticService = null,
            IRequestContext requestContext = null,
            ICachingService cachingService = null,
            IAuthorizationDataAccess dataAccess = null,
            AuthorizationSection authorizationSection = null)
        {
            _authDataAccess = dataAccess ?? new AuthorizationDataAccess();
            _statisticService = statisticService ?? new StatisticService();
            _cachingService = cachingService ?? new CachingService();
            _requestContext = requestContext ?? new RequestContext();
            _authorizationSection = authorizationSection ?? AuthorizationSection.Instance;
            _cachedExpireTime = _authorizationSection.CachedExpireTime;
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
            var groupPageAccesses = _authorizationSection.GroupPageAccesses.Cast<GroupPageAccessElement>()
                .Where(group => group.RoleId <= _requestContext.UserSession.RoleId);

            foreach (GroupPageAccessElement groupPageAccess in groupPageAccesses)
            {
                var pageAccesses = groupPageAccess.PageAccesses.Cast<PageAccessElement>()
                    .Where(pageAccess => _requestContext.Url.ToLower().Contains(pageAccess.Url.ToLower()));

                foreach (PageAccessElement pageAccess in pageAccesses)
                {
                    if (_requestContext.CustId != 0
                        && pageAccess.IsCheckBelonging)
                    {
                        return IsBelongingOnPage(pageAccess);
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

                : _authDataAccess.IsBelonging(
                    _requestContext.CustId,
                    _requestContext.UserSession.CustId,
                    _requestContext.UserSession.SubAccId,
                    _requestContext.TransId,
                    _requestContext.UserSession.IsInternal);
        }

        public bool IsAuthorized(string url = null)
        {
            url = url ?? _requestContext.Url;

            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            var permission = _authorizationSection.FullPermission;

            if (_requestContext.UserSession.SubAccId > 0)
            {
                permission = _requestContext.UserSession.SubPageAccess;
            }

            return IsAuthorizedOnGroupOfPage(
                permission,
                url);
        }

        private bool IsAuthorizedOnGroupOfPage(
            string customerPermission,
            string url)
        {
            var groupPageAccesses = _authorizationSection.GroupPageAccesses.Cast<GroupPageAccessElement>()
                .Where(group => group.RoleId <= _requestContext.UserSession.RoleId);

            foreach (var groupPageAccess in groupPageAccesses)
            {
                var pageAccesses = groupPageAccess.PageAccesses.Cast<PageAccessElement>()
                    .Where(pageAccess => url.ToLower().Contains(pageAccess.Url.ToLower()));

                foreach (var pageAccess in pageAccesses)
                {
                    var isAuthorized = IsAuthorizedOnPage(customerPermission, pageAccess);

                    if (isAuthorized)
                    {
                        if (!_requestContext.UserSession.IsInternal)
                        {
                            //// Update page visit if user has permission and is not internal user.
                            _statisticService.UpdateFunctionUsage(
                                pageAccess.Url,
                                pageAccess.MainFunctionName,
                                pageAccess.SubFunctionName,
                                _requestContext.UserSession.Site);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsAuthorizedOnPage(string customerPermission, PageAccessElement pageElement)
        {
            var permissions = pageElement.Permission.Split(',');

            foreach (var permission in permissions)
            {
                if (customerPermission.Contains(permission))
                {
                    return true;
                }
            }

            return false;
        }

        private string GetSubPermission()
        {
            var subAccountPermission = _cachingService.GetOrAdd(
                string.Format(SubAccountPermissionCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId),
                () =>
                {
                    return _authDataAccess.GetSubAccountPermission(_requestContext.UserSession.SubAccId);
                },
                _cachedExpireTime);

            return subAccountPermission;
        }

        public bool IsKickedOut()
        {
            if (_requestContext.UserSession.IsInternal)
            {
                return false;
            }

            if (_authorizationSection.IgnoreRecheckinCustIds.Contains(_requestContext.UserSession.CustId))
            {
                return false;
            }

            var onlineUserId = _requestContext.UserSession.OnUserId;

            if (onlineUserId <= 0)
            {
                return true;
            }

            var isKickedOut = _cachingService.GetOrAdd(
                string.Format(RecheckInCacheKey, _requestContext.UserSession.CustId, _requestContext.UserSession.SubAccId, onlineUserId),
                () =>
                {
                    return _authDataAccess.IsKickedOut(onlineUserId);
                },
                _cachedExpireTime);

            return isKickedOut;
        }
    }
}