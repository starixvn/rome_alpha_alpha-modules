namespace Alpha.Web.Helpers
{
    using System;
    using System.Linq;
    using Configurations;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public interface NicknameChecker
    {
        string ForceLoginDate { get; }

        bool IsOfferForceLogin { get; }

        bool IsOfferForceLoginFlow { get; }

        bool IsRecommendLoginByNickname { get; }

        bool IsForceLoginByNickname { get; }

        bool IsOfferForceLoginAndOverDueDate { get; }
    }

    public class NicknameCheckerImpl : NicknameChecker
    {
        private readonly IUserSession userSession;

        private bool? isOfferForceLogin;
        private bool? isOfferForceLoginFlow;
        private bool? isRecommendLoginByNickname;
        private bool? isForceLoginByNickname;
        private bool? isOfferForceLoginAndOverDueDate;

        public NicknameCheckerImpl(IUserSession userSession = null)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public string ForceLoginDate
        {
            get
            {
                var forceLoginDate = GetForceLoginDate();

                return forceLoginDate == null
                    ? string.Empty
                    : forceLoginDate.Value.ToString("dd/MM/yyyy");
            }
        }

        public bool IsOfferForceLogin
        {
            get
            {
                if (isOfferForceLogin == null)
                {
                    if (GetForceLoginDate() == null)
                    {
                        isOfferForceLogin = false;
                    }
                    else
                    {
                        var isIgnore = AppSettings.ForceLoginNicknameIgnoreList.Count > 0
                            && new[]
                            {
                                userSession.SRecommendId,
                                userSession.MRecommendId,
                                userSession.ARecommendId,
                                userSession.CustId
                            }.Any(id => id != 0 && AppSettings.ForceLoginNicknameIgnoreList.ContainsKey(id));

                        isOfferForceLogin = !isIgnore;
                    }
                }

                return isOfferForceLogin.Value;
            }
        }

        public bool IsOfferForceLoginFlow
        {
            get
            {
                if (isOfferForceLoginFlow == null)
                {
                    if (userSession.IsInternal || GetForceLoginDate() == null)
                    {
                        isOfferForceLoginFlow = false;
                    }
                    else
                    {
                        var isIgnore = AppSettings.ForceLoginNicknameIgnoreList.Count > 0
                            && new[]
                            {
                                userSession.SRecommendId,
                                userSession.MRecommendId,
                                userSession.ARecommendId,
                                userSession.CustId
                            }.Any(id => id != 0 && AppSettings.ForceLoginNicknameIgnoreList.ContainsKey(id));

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
                isRecommendLoginByNickname = isRecommendLoginByNickname ?? IsOfferForceLoginFlow && GetForceLoginDate().Value > DateTime.Today;

                return isRecommendLoginByNickname.Value;
            }
        }

        public bool IsForceLoginByNickname
        {
            get
            {
                isForceLoginByNickname = isForceLoginByNickname ?? IsOfferForceLoginFlow && GetForceLoginDate().Value <= DateTime.Today;

                return isForceLoginByNickname.Value;
            }
        }

        public bool IsOfferForceLoginAndOverDueDate
        {
            get
            {
                isOfferForceLoginAndOverDueDate = isOfferForceLoginAndOverDueDate ?? IsOfferForceLogin && GetForceLoginDate().Value <= DateTime.Today;

                return isOfferForceLoginAndOverDueDate.Value;
            }
        }

        private DateTime? GetForceLoginDate()
            => userSession.IsSubAcc
            ? AppSettings.ForceLoginNicknameDate4SubAccount
            : AppSettings.ForceLoginNicknameDate;
    }
}
