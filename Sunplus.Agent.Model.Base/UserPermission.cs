namespace Sunplus.Agent.Model.Entities.Permission
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities.Base;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public class UserPermission : IEntity
    {
        private static string neutralSite = "789y,"; // Seperate by comma
        private static string creditNeutralSite = "789y,"; // Seperate by comma

        public static bool IsDisabledHR
        {
            get
            {
                return false;
            }
        }

        public static bool IsDisabledFI
        {
            get
            {
                return true;
            }
        }

        #region Racing

        public static bool IsShowRacing
        {
            get
            {
                return IsShowRacing_151_31 |
                    IsShowRacing_151_41 |
                    IsShowRacing_151_51 |
                    IsShowRacing_152_41 |
                    IsShowRacing_152_51 |
                    IsShowRacing_153_51;
            }
        }

        public static bool IsShowRacing_151_31
        {
            get
            {
                if (AppSettings.ISPOSuperId == CurrentUserSession.SRecommendId || AppSettings.SpecialSuperId == CurrentUserSession.SRecommendId)
                {
                    return true;
                }

                return !AppSettings.Disable151_31;
            }
        }

        public static bool IsShowRacing_151_41
        {
            get
            {
                return !AppSettings.Disable151_41;
            }
        }

        public static bool IsShowRacing_151_51
        {
            get
            {
                return !AppSettings.Disable151_51;
            }
        }

        public static bool IsShowRacing_152_41
        {
            get
            {
                return !AppSettings.Disable152_41;
            }
        }

        public static bool IsShowRacing_152_51
        {
            get
            {
                return !AppSettings.Disable152_51;
            }
        }

        public static bool IsShowRacing_153_51
        {
            get
            {
                return !AppSettings.Disable153_51;
            }
        }

        #endregion Racing

        public static bool IsShowNumberGroup
        {
            get
            {
                return AppSettings.EnableNumberGroup;
            }
        }

        public static bool IsShowFinance
        {
            get
            {
                return false;
            }
        }

        public static string NeutralSite
        {
            get { return UserPermission.neutralSite; }
            set { UserPermission.neutralSite = value; }
        }

        /// <summary>
        /// Indicate logged-in customer is neutral or not.
        /// </summary>
        public static bool IsNeutral
        {
            get
            {
                return AppSettings.IsNeutralSite ||
                    (CurrentUserSession.Site != null && neutralSite.IndexOf(CurrentUserSession.Site, 0, StringComparison.CurrentCultureIgnoreCase) != -1);
            }
        }

        public static bool IsShowCasino // Already offer for 789y site
        {
            get
            {
                return CurrentUserSession.IsCasinoSync;
            }
        }

        public static bool IsShowBingo
        {
            get
            {
                return AppSettings.IsEnableBingo && CurrentUserSession.IsBingoSync;
            }
        }

        public static bool IsShowBingoReport
        {
            get
            {
                return (!IsNeutral ||
                   (CurrentUserSession.Site != null && creditNeutralSite.IndexOf(CurrentUserSession.Site, 0, StringComparison.CurrentCultureIgnoreCase) != -1))
                   && CurrentUserSession.IsBingoSync;
            }
        }

        public static string CreditNeutralSite
        {
            get { return UserPermission.creditNeutralSite; }
            set { UserPermission.creditNeutralSite = value; }
        }

        public static bool IsShowNumberGame
        {
            get
            {
                // Offer for CreditNeutral
                return (!IsNeutral ||
                    (CurrentUserSession.Site != null && creditNeutralSite.IndexOf(CurrentUserSession.Site, 0, StringComparison.CurrentCultureIgnoreCase) != -1))
                    && !AppSettings.DisabledNumberGame;
            }
        }

        public static bool IsShowMixedSport
        {
            get
            {
                return AppSettings.IsShowMixedSport;
            }
        }

        public static bool IsShowP2P
        {
            get
            {
                return /*!IsNeutral && */AppSettings.IsEnableP2P && !CurrentUserSession.IsP2PDisabled;
            }
        }

        public static bool IsShowLiveCasino
        {
            get
            {
                return AppSettings.IsEnableLiveCasino && CurrentUserSession.IsLiveCasinoSync;
            }
        }

        public static bool IsShowVirtualSports
        {
            get
            {
                return AppSettings.IsEnableVirtualSports && !CurrentUserSession.IsHideVirtualSports;
            }
        }

        public static bool IsShowKenoSetting
        {
            get
            {
                return KenoSection.Instance.ShowSetting && !CurrentUserSession.IsHideKeno;
            }
        }

        public static bool IsShowGoldDeluxeSettingForCredit
        {
            get
            {
                return AppSettings.IsShowGoldDeluxeSettingForCredit && !CurrentUserSession.IsHideGoldDeluxe;
            }
        }

        public static bool IsShowColossusBets
        {
            get
            {
                return AppSettings.IsShowColossusBets && !CurrentUserSession.IsHideColossusBets;
            }
        }

        public static bool IsShowSportsbook2
        {
            get
            {
                return AppSettings.IsShowSportsbook2 && !CurrentUserSession.IsHideSportsbook2;
            }
        }

        public static bool IsShowHRFixedOdds
        {
            get
            {
                return AppSettings.IsEnableHRFixedOdds && !CurrentUserSession.IsHideHRFixedOdds;
            }
        }

        public static bool IsShowDoubleCommission
        {
            get
            {
                return AppSettings.IsEnableDoubleCommission && CurrentUserSession.IsDoubleCommLosing;
            }
        }

        public static bool EnableKeno
        {
            get
            {
                return KenoSection.Instance.Enable && !CurrentUserSession.IsHideKeno;
            }
        }

        public static bool IsShowSecurityVerify
        {
            get
            {
                //// 1. site is enabled or not
                /// ignore case sensitive
                string site = CurrentUserSession.Site.ToLower();
                var enabled = AppSettings.EnableSecurityVerify.Contains(site);
                if (!enabled)
                {
                    return false;
                }

                //// 2. deadline
                var expiredDate = DateTime.Parse(ConfigurationManager.AppSettings["DeadlineEnableSecurity"]);
                var deadline = DateTime.Now > expiredDate;
                if (deadline)
                {
                    return true;
                }

                //// 3. others
                var forced = CurrentUserSession.IsForcedSecurityCode;
                var scEmpty = string.IsNullOrEmpty(CurrentUserSession.SecurityCode);
                if (forced)
                {
                    return true;
                }
                //// Not forced
                return !scEmpty;
            }
        }

        public static bool IsShowMyanmarOdds
        {
            get
            {
                return AppSettings.IsSupportMyanmarOdds && CurrentUserSession.IsMyanmarOdds;
            }
        }

        public static bool IsIMBCCustomer
        {
            get
            {
                return CurrentUserSession.IsIMBCCustomer;
            }
        }

        public static bool IsShowSuperLive
        {
            get
            {
                return !CurrentUserSession.IsDisabledSuperLive;
            }
        }

        protected static IUserSession CurrentUserSession
        {
            get
            {
                return Sunplus.Agent.Model.Entities.UserSession.Current;
            }
        }

        public static bool IsSyncAllBet
		{
			get
			{
				return CurrentUserSession.IsSyncedAllBet;
			}
		}

        public static bool IsEnableProduct(int product)
        {
            var hideProducts = CurrentUserSession.HideProducts;

            return AppSettings.IsEnabledProduct(product) &&
                hideProducts.ContainsKey(product) &&
                !hideProducts[product];
        }

        public static bool IsSyncedProduct(int product)
        {
            var syncProducts = CurrentUserSession.SyncProducts;

            return syncProducts.ContainsKey(product) && syncProducts[product];
        }
    }
}