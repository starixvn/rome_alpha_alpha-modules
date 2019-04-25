namespace Alpha.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public interface NicknameChecker
    {
        string ForceLoginDate { get; }

        bool IsOfferForceLoginFlow { get; }

        bool IsRecommendLoginByNickname { get; }

        bool IsForceLoginByNickname { get; }
    }

    public class NicknameCheckerImpl : NicknameChecker
    {
        private readonly DateTime? forceLoginDate;
        private readonly IDictionary<int, bool> forceLoginIgnoreList;
        private readonly IUserSession userSession;

        private bool? isOfferForceLoginFlow;
        private bool? isRecommendLoginByNickname;
        private bool? isForceLoginByNickname;

        public NicknameCheckerImpl(
            DateTime? forceLoginDate = null,
            IDictionary<int, bool> forceLoginIgnoreList = null,
            IUserSession userSession = null)
        {
            this.forceLoginDate = forceLoginDate ?? AppSettings.ForceLoginNicknameDate;
            this.forceLoginIgnoreList = forceLoginIgnoreList ?? AppSettings.ForceLoginNicknameIgnoreList;
            this.userSession = userSession ?? new UserSessionExt();
        }

        public string ForceLoginDate
            => forceLoginDate == null
            ? string.Empty
            : forceLoginDate.Value.ToString("dd/MM/yyyy");

        public bool IsOfferForceLoginFlow
        {
            get
            {
                if (isOfferForceLoginFlow == null)
                {
                    if (userSession.IsInternal || userSession.IsSubAcc || forceLoginDate == null)
                    {
                        isOfferForceLoginFlow = false;
                    }
                    else
                    {
                        var isIgnore = forceLoginIgnoreList.Count > 0
                            && new[]
                            {
                                userSession.SRecommendId,
                                userSession.MRecommendId,
                                userSession.ARecommendId,
                                userSession.CustId
                            }.Any(id => id != 0 && forceLoginIgnoreList.ContainsKey(id));

                        isOfferForceLoginFlow = isIgnore
                            ? false
                            : !(!string.IsNullOrWhiteSpace(userSession.NickName) && userSession.IsNicknameLoginOnly);
                    }
                }

                return isOfferForceLoginFlow.Value;
            }
        }

        public bool IsRecommendLoginByNickname
        {
            get
            {
                isRecommendLoginByNickname = isRecommendLoginByNickname ?? IsOfferForceLoginFlow && forceLoginDate.Value > DateTime.Today;

                return isRecommendLoginByNickname.Value;
            }
        }

        public bool IsForceLoginByNickname
        {
            get
            {
                isForceLoginByNickname = isForceLoginByNickname ?? IsOfferForceLoginFlow && forceLoginDate.Value <= DateTime.Today;

                return isForceLoginByNickname.Value;
            }
        }
    }
}
