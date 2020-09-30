namespace Sunplus.Agent.Model.Entities.Permission
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using Definitions;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities.Base;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public class UserPermission : IEntity
    {
        private static string neutralSite = "789y,"; // Seperate by comma
        private static string creditNeutralSite = "789y,"; // Seperate by comma
        
        public static bool IsShowNumberGroup
        {
            get
            {
                return AppSettings.EnableNumberGroup;
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
                return CurrentUserSession.GetSyncedProduct(ProductIdConstants.RNGCasino);
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

        public static bool IsShowKenoSetting
        {
            get
            {
                return AppSettings.IsEnableKeno && !CurrentUserSession.IsHideKeno;
            }
        }

        public static bool IsShowGoldDeluxeSettingForCredit
        {
            get
            {
                return AppSettings.IsShowGoldDeluxeSettingForCredit && !CurrentUserSession.GetHideProduct(ProductIdConstants.GoldDeluxe);
            }
        }

        public static bool IsShowSportsbook2
        {
            get
            {
                return AppSettings.IsShowSportsbook2 && !CurrentUserSession.GetHideProduct(ProductIdConstants.Sportsbook2);
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
                return AppSettings.IsEnableKeno && !CurrentUserSession.IsHideKeno;
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
                return UserSession.Current;
            }
        }

        public static bool IsEnableProduct(int product)
        {
            return AppSettings.IsEnabledProduct(product) && !CurrentUserSession.GetHideProduct(product);
        }

        public static bool IsSyncedProduct(int product)
        {
            return CurrentUserSession.GetSyncedProduct(product);
        }

        public static bool IsDisabledProduct(int product)
        {
            return CurrentUserSession.GetDisabledProduct(product);
        }

        public static bool HasPermissionOnProduct(int productId)
        {
            switch (productId)
            {
                case 1: // sportsbook
                    return true;
                case 3: // Number Game
                    return IsShowNumberGame;
                case 6: // Casino
                    return IsShowCasino;
                case 8: // Keno
                    return IsShowKenoSetting;
                case 12: // Gold Deluxe
                    return IsShowGoldDeluxeSettingForCredit;
                case 16: // SP2
                    return IsShowSportsbook2;
                case 5: // Virtual Sports
                case 44: // RNG Lottery
                    return IsEnableProduct(productId);
                default:
                    return IsEnableProduct(productId) && IsSyncedProduct(productId);
            }
        }
    }
}