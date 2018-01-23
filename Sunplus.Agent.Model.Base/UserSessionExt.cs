namespace Sunplus.Agent.Model.Base
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using BL.Definitions;
    using Entities;

    public class UserSessionExt : IUserSession
    {
        public int AdminId
        {
            get
            {
                return UserSession.Current.AdminId;
            }
        }

        public string AdminName
        {
            get
            {
                return UserSession.Current.AdminName;
            }
        }

        public int ARecommendId
        {
            get
            {
                return UserSession.Current.ARecommendId;
            }
        }

        public string CaptchaCode
        {
            get
            {
                return UserSession.Current.CaptchaCode;
            }

            set
            {
                UserSession.Current.CaptchaCode = value;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return UserSession.Current.Culture;
            }
        }

        public string Currency
        {
            get
            {
                return UserSession.Current.Currency;
            }
        }

        public int CurrencyId
        {
            get
            {
                return UserSession.Current.CurrencyId;
            }
        }

        public int CustId
        {
            get
            {
                return UserSession.Current.CustId;
            }
        }

        public bool DisableColossusBets
        {
            get
            {
                return UserSession.Current.DisableColossusBets;
            }

            set
            {
                UserSession.Current.DisableColossusBets = value;
            }
        }

        public bool DisableGoldDeluxe
        {
            get
            {
                return UserSession.Current.DisableGoldDeluxe;
            }

            set
            {
                UserSession.Current.DisableGoldDeluxe = value;
            }
        }

        public bool DisableSportsbook2
        {
            get
            {
                return UserSession.Current.DisableSportsbook2;
            }

            set
            {
                UserSession.Current.DisableSportsbook2 = value;
            }
        }

        public string DisplayNickName
        {
            get
            {
                return UserSession.Current.DisplayNickName;
            }

            set
            {
                UserSession.Current.DisplayNickName = value;
            }
        }

        public bool IsAdminCanViewVNIP
        {
            get
            {
                return UserSession.Current.IsAdminCanViewVNIP;
            }
        }

        public bool IsAgent
        {
            get
            {
                return UserSession.Current.IsAgent;
            }
        }

        public bool IsAllowedTotalBalanceTransfer
        {
            get
            {
                return UserSession.Current.IsAllowedTotalBalanceTransfer;
            }
        }

        public bool IsAllowHRCurrency
        {
            get
            {
                return UserSession.Current.IsAllowHRCurrency;
            }
        }

        public bool IsBelongTo9Currency
        {
            get
            {
                return UserSession.Current.IsBelongTo9Currency;
            }

            set
            {
                UserSession.Current.IsBelongTo9Currency = value;
            }
        }  

        public bool IsCasinoDisabled
        {
            get
            {
                return UserSession.Current.IsCasinoDisabled;
            }

            set
            {
                UserSession.Current.IsCasinoDisabled = value;
            }
        }

        public bool IsCasinoSync
        {
            get
            {
                return UserSession.Current.IsCasinoSync;
            }

            set
            {
                UserSession.Current.IsCasinoSync = value;
            }
        }

        public bool IsDisable151_31
        {
            get
            {
                return UserSession.Current.IsDisable151_31;
            }
        }

        public bool IsDisable151_41
        {
            get
            {
                return UserSession.Current.IsDisable151_41;
            }
        }

        public bool IsDisable151_51
        {
            get
            {
                return UserSession.Current.IsDisable151_51;
            }
        }

        public bool IsDisable152_41
        {
            get
            {
                return UserSession.Current.IsDisable152_41;
            }
        }

        public bool IsDisable152_51
        {
            get
            {
                return UserSession.Current.IsDisable152_51;
            }
        }

        public bool IsDisable153_51
        {
            get
            {
                return UserSession.Current.IsDisable153_51;
            }
        }

        public bool IsDisabledAutoMPT
        {
            get
            {
                return UserSession.Current.IsDisabledAutoMPT;
            }
        }

        public bool IsShowBetChoiceLimit
        {
            get
            {
                return UserSession.Current.IsShowBetChoiceLimit;
            }

            set
            {
                UserSession.Current.IsShowBetChoiceLimit = value;
            }
        }

        public bool IsDisabledESports
        {
            get
            {
                return UserSession.Current.IsDisabledESports;
            }

            set
            {
                UserSession.Current.IsDisabledESports = value;
            }
        }

        public bool IsDisabledFI
        {
            get
            {
                return UserSession.Current.IsDisabledFI;
            }
        }

        public bool IsDisabledHR
        {
            get
            {
                return UserSession.Current.IsDisabledHR;
            }
        }
       
        public bool IsDisabledSuperLive
        {
            get
            {
                return UserSession.Current.IsDisabledSuperLive;
            }

            set
            {
                UserSession.Current.IsDisabledSuperLive = value;
            }
        }

        public bool IsDisabledVirtualSports
        {
            get
            {
                return UserSession.Current.IsDisabledVirtualSports;
            }

            set
            {
                UserSession.Current.IsDisabledVirtualSports = value;
            }
        }

        public bool IsDoubleCommLosing
        {
            get
            {
                return UserSession.Current.IsDoubleCommLosing;
            }

            set
            {
                UserSession.Current.IsDoubleCommLosing = value;
            }
        }

        public bool IsForcedSecurityCode
        {
            get
            {
                return UserSession.Current.IsForcedSecurityCode;
            }

            set
            {
                UserSession.Current.IsForcedSecurityCode = value;
            }
        }

        public bool IsHideColossusBets
        {
            get
            {
                return UserSession.Current.IsHideColossusBets;
            }

            set
            {
                UserSession.Current.IsHideColossusBets = value;
            }
        }

        public bool IsHideGoldDeluxe
        {
            get
            {
                return UserSession.Current.IsHideGoldDeluxe;
            }

            set
            {
                UserSession.Current.IsHideGoldDeluxe = value;
            }
        }       

        public bool IsHideKeno
        {
            get
            {
                return UserSession.Current.IsHideKeno;
            }

            set
            {
                UserSession.Current.IsHideKeno = value;
            }
        }

        public bool IsHideSportsbook2
        {
            get
            {
                return UserSession.Current.IsHideSportsbook2;
            }

            set
            {
                UserSession.Current.IsHideSportsbook2 = value;
            }
        }

        public bool IsHideVirtualSports
        {
            get
            {
                return UserSession.Current.IsHideVirtualSports;
            }

            set
            {
                UserSession.Current.IsHideVirtualSports = value;
            }
        }

        public bool IsIMBCCustomer
        {
            get
            {
                return UserSession.Current.IsIMBCCustomer;
            }

            set
            {
                UserSession.Current.IsIMBCCustomer = value;
            }
        }

        public bool IsInternal
        {
            get
            {
                return UserSession.Current.IsInternal;
            }
        }

        public bool IsInternalWebVPN
        {
            get
            {
                return UserSession.Current.IsInternalWebVPN;
            }

            set
            {
                UserSession.Current.IsInternalWebVPN = value;
            }
        }

        public bool IsLimitedHostPermisionOnSubAccountFunction
        {
            get
            {
                return UserSession.Current.IsLimitedHostPermisionOnSubAccountFunction;
            }

            set
            {
                UserSession.Current.IsLimitedHostPermisionOnSubAccountFunction = value;
            }
        }

        public bool IsLiveCasinoDisabled
        {
            get
            {
                return UserSession.Current.IsLiveCasinoDisabled;
            }

            set
            {
                UserSession.Current.IsLiveCasinoDisabled = value;
            }
        }

        public bool IsLiveCasinoSync
        {
            get
            {
                return UserSession.Current.IsLiveCasinoSync;
            }

            set
            {
                UserSession.Current.IsLiveCasinoSync = value;
            }
        }

        public bool IsLogged
        {
            get
            {
                return UserSession.Current.IsLogged;
            }

            set
            {
                UserSession.Current.IsLogged = value;
            }
        }

        public bool IsMaster
        {
            get
            {
                return UserSession.Current.IsMaster;
            }
        }

        public bool IsMyanmarOdds
        {
            get
            {
                return UserSession.Current.IsMyanmarOdds;
            }

            set
            {
                UserSession.Current.IsMyanmarOdds = value;
            }
        }

        public bool IsNickname
        {
            get
            {
                return UserSession.Current.IsNickname;
            }

            set
            {
                UserSession.Current.IsNickname = value;
            }
        }

        public bool IsNicknameLoginOnly
        {
            get
            {
                return UserSession.Current.IsNicknameLoginOnly;
            }

            set
            {
                UserSession.Current.IsNicknameLoginOnly = value;
            }
        }

        public bool IsNotCurrencyCasino
        {
            get
            {
                return UserSession.Current.IsNotCurrencyCasino;
            }

            set
            {
                UserSession.Current.IsNotCurrencyCasino = value;
            }
        }

        public bool IsNumberGameDisabled
        {
            get
            {
                return UserSession.Current.IsNumberGameDisabled;
            }
        }       

        public bool IsPasswordExpired
        {
            get
            {
                return UserSession.Current.IsPasswordExpired;
            }

            set
            {
                UserSession.Current.IsPasswordExpired = value;
            }
        }

        public bool IsShowConsolidatedReport
        {
            get
            {
                return UserSession.Current.IsShowConsolidatedReport;
            }

            set
            {
                UserSession.Current.IsShowConsolidatedReport = value;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return UserSession.Current.IsSubAcc;
            }
        }

        public bool IsSuper
        {
            get
            {
                return UserSession.Current.IsSuper;
            }
        }

        public string LangKey
        {
            get
            {
                return UserSession.Current.LangKey;
            }

            set
            {
                UserSession.Current.LangKey = value;
            }
        }

        public string Language
        {
            get
            {
                return UserSession.Current.Language;
            }

            set
            {
                UserSession.Current.Language = value;
            }
        }

        public DateTime LastChangedPassword
        {
            get
            {
                return UserSession.Current.LastChangedPassword;
            }

            set
            {
                UserSession.Current.LastChangedPassword = value;
            }
        }

        public int MRecommendId
        {
            get
            {
                return UserSession.Current.MRecommendId;
            }
        }

        public string NetSessionId
        {
            get
            {
                return UserSession.Current.NetSessionId;
            }
        }

        public string NickName
        {
            get
            {
                return UserSession.Current.NickName;
            }

            set
            {
                UserSession.Current.NickName = value;
            }
        }

        public int OnUserId
        {
            get
            {
                return UserSession.Current.OnUserId;
            }
        }

        public byte RoleId
        {
            get
            {
                return UserSession.Current.RoleId;
            }
        }

        public string SecurityCode
        {
            get
            {
                return UserSession.Current.SecurityCode;
            }

            set
            {
                UserSession.Current.SecurityCode = value;
            }
        }

        public SCResult SecurityCodeResult
        {
            get
            {
                return UserSession.Current.SecurityCodeResult;
            }

            set
            {
                UserSession.Current.SecurityCodeResult = value;
            }
        }

        public string SecurityCodeToken
        {
            get
            {
                return UserSession.Current.SecurityCodeToken;
            }

            set
            {
                UserSession.Current.SecurityCodeToken = value;
            }
        }

        public int ServerId
        {
            get
            {
                return UserSession.Current.ServerId;
            }
        }

        public bool ShowSecurityCodePopup
        {
            get
            {
                return UserSession.Current.ShowSecurityCodePopup;
            }

            set
            {
                UserSession.Current.ShowSecurityCodePopup = value;
            }
        }

        public string Site
        {
            get
            {
                return UserSession.Current.Site;
            }
        }

        public string SiteName
        {
            get
            {
                return UserSession.Current.SiteName;
            }
        }

        public int SRecommendId
        {
            get
            {
                return UserSession.Current.SRecommendId;
            }
        }

        public int SubAccId
        {
            get
            {
                return UserSession.Current.SubAccId;
            }
        }

        public PageAccess SubPageAccess
        {
            get
            {
                return UserSession.Current.SubPageAccess;
            }
        }

        public string SubUserName
        {
            get
            {
                return UserSession.Current.SubUserName;
            }
        }

        public int UserId
        {
            get
            {
                return UserSession.Current.UserId;
            }
        }

        public string UserLoginIp
        {
            get
            {
                return UserSession.Current.UserLoginIp;
            }

            set
            {
                UserSession.Current.UserLoginIp = value;
            }
        }

        public string UserName
        {
            get
            {
                return UserSession.Current.UserName;
            }
        }

        public bool IsAllDayBalTransfer
        {
            get
            {
                return UserSession.Current.IsAllDayBalTransfer;
            }
        }

        public bool GetDisabledProduct(int productId)
        {
            return UserSession.Current.GetDisabledProduct(productId);
        }

        public bool GetHideProduct(int productId)
        {
            return UserSession.Current.GetHideProduct(productId);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            UserSession.Current.GetObjectData(info, context);
        }

        public bool GetSyncedProduct(int productId)
        {
            return UserSession.Current.GetSyncedProduct(productId);
        }

        public void SetDisabledProduct(int productId, bool value)
        {
            UserSession.Current.SetDisabledProduct(productId, value);
        }

        public void SetHideProduct(int productId, bool value)
        {
            UserSession.Current.SetHideProduct(productId, value);
        }

        public void SetSyncedProduct(int productId, bool value)
        {
            UserSession.Current.SetSyncedProduct(productId, value);
        }
    }
}