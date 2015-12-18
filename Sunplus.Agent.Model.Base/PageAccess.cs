namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Runtime.Serialization;
    using Sunplus.Agent.Model.Entities.Base;

    [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public struct PageAccessType
    {
        public const string CreateAccount = "[A]";
        public const string UpdateAccount = "[B]";
        public const string Transfer = "[C]";
        public const string ViewTotalBets = "[D]";
        public const string ViewReports = "[E]";
        public const string ViewLog = "[G]";
        public const string ViewMemberInfo = "[H]";

        public static bool IsValidType(string type)
        {
            return type == CreateAccount ||
                type == UpdateAccount ||
                type == Transfer ||
                type == ViewTotalBets ||
                type == ViewReports ||
                type == ViewLog ||
                type == ViewMemberInfo;
        }
    }

    /// <summary>
    /// Indicate permissions of a sub-account.
    /// </summary>
    [Serializable]
    [Obsolete("Should rename to Sunplus.Agent.Model.UserSession")]
    public class PageAccess : ISerializable, IEntity
    {
        private bool isCreateAccount = false;
        private bool isUpdateAccount = false;
        private bool isTransfer = false;
        private bool isViewTotalBets = false;
        private bool isViewReports = false;
        private bool isViewLog = false;
        private bool isViewMemberInfo = false;

        public PageAccess()
        {
        }

        public PageAccess(SerializationInfo info, StreamingContext context)
        {
            this.isCreateAccount = info.GetBoolean("_createAccount");
            this.isTransfer = info.GetBoolean("_transfer");
            this.isUpdateAccount = info.GetBoolean("_updateAccount");
            this.isViewReports = info.GetBoolean("_viewReports");
            this.isViewTotalBets = info.GetBoolean("_viewTotalBets");

            try
            {
                this.isViewLog = info.GetBoolean("_viewLog");
                this.isViewMemberInfo = info.GetBoolean("_viewMemberInfo");
            }
            catch
            {
                this.isViewLog = this.isUpdateAccount;
                this.isViewMemberInfo = this.isUpdateAccount;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [A] to create an account.
        /// </summary>
        public bool CreateAccount
        {
            get
            {
                return this.isCreateAccount;
            }

            set
            {
                this.isCreateAccount = value;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [B] to update an account.
        /// </summary>
        public bool UpdateAccount
        {
            get
            {
                return this.isUpdateAccount;
            }

            set
            {
                this.isUpdateAccount = value;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [C] to transfer credit.
        /// </summary>
        public bool Transfer
        {
            get
            {
                return this.isTransfer;
            }

            set
            {
                this.isTransfer = value;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [D] to view total bets.
        /// </summary>
        public bool ViewTotalBets
        {
            get
            {
                return this.isViewTotalBets;
            }

            set
            {
                this.isViewTotalBets = value;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [E] to view reports.
        /// </summary>
        public bool ViewReports
        {
            get
            {
                return this.isViewReports;
            }

            set
            {
                this.isViewReports = value;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [G] to view log.
        /// </summary>
        public bool ViewLog
        {
            get
            {
                return this.isViewLog;
            }

            set
            {
                this.isViewLog = value;
            }
        }

        /// <summary>
        /// Indicate that sub-account has permission [H] to view reports.
        /// </summary>
        public bool ViewMemberInfo
        {
            get
            {
                return this.isViewMemberInfo;
            }

            set
            {
                this.isViewMemberInfo = value;
            }
        }

        /// <summary>
        /// Implicit convert a page access to a string.
        /// </summary>
        /// <param name="pa"></param>
        /// <returns></returns>
        public static implicit operator string(PageAccess value)
        {
            return (value.isCreateAccount ? PageAccessType.CreateAccount : string.Empty)
                + (value.isUpdateAccount ? PageAccessType.UpdateAccount : string.Empty)
                + (value.isTransfer ? PageAccessType.Transfer : string.Empty)
                + (value.isViewTotalBets ? PageAccessType.ViewTotalBets : string.Empty)
                + (value.isViewReports ? PageAccessType.ViewReports : string.Empty)
                + (value.isViewLog ? PageAccessType.ViewLog : string.Empty)
                + (value.isViewMemberInfo ? PageAccessType.ViewMemberInfo : string.Empty);
        }

        /// <summary>
        /// Implicit convert a string to a page access.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator PageAccess(string value)
        {
            PageAccess result = new PageAccess();

            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            result.isCreateAccount = value.Contains(PageAccessType.CreateAccount);
            result.isUpdateAccount = value.Contains(PageAccessType.UpdateAccount);
            result.isTransfer = value.Contains(PageAccessType.Transfer);
            result.isViewTotalBets = value.Contains(PageAccessType.ViewTotalBets);
            result.isViewReports = value.Contains(PageAccessType.ViewReports);
            result.isViewLog = value.Contains(PageAccessType.ViewLog);
            result.isViewMemberInfo = value.Contains(PageAccessType.ViewMemberInfo);

            return result;
        }

        public bool IsEmptyPermission()
        {
            return !(this.isCreateAccount || this.isUpdateAccount || this.isTransfer || this.isViewTotalBets || this.isViewReports || this.isViewLog || this.isViewMemberInfo);
        }

        /// <summary>
        /// Check this page access contains other page access.
        /// </summary>
        /// <param name="value"></param>
        public bool Contains(PageAccess value)
        {
            return ((this.CreateAccount && value.CreateAccount) || (this.CreateAccount == value.CreateAccount == false))
                && ((this.UpdateAccount && value.UpdateAccount) || (this.UpdateAccount == value.UpdateAccount == false))
                && ((this.Transfer && value.Transfer) || (this.Transfer == value.Transfer == false))
                && ((this.ViewTotalBets && value.ViewTotalBets) || (this.ViewTotalBets == value.Transfer == false))
                && ((this.ViewReports && value.ViewReports) || (this.ViewReports == value.ViewReports == false))
                && ((this.ViewLog && value.ViewLog) || (this.ViewLog == value.ViewLog == false))
                && ((this.ViewMemberInfo && value.ViewMemberInfo) || (this.ViewMemberInfo == value.ViewMemberInfo == false));
        }

        #region ISerializable Members

        public void SetFullPermission()
        {
            this.isCreateAccount = this.isUpdateAccount = this.isTransfer = this.isViewTotalBets = this.isViewReports = this.isViewLog = this.isViewMemberInfo = true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_createAccount", this.isCreateAccount);
            info.AddValue("_transfer", this.isTransfer);
            info.AddValue("_updateAccount", this.isUpdateAccount);
            info.AddValue("_viewReports", this.isViewReports);
            info.AddValue("_viewTotalBets", this.isViewTotalBets);
            info.AddValue("_viewLog", this.isViewLog);
            info.AddValue("_viewMemberInfo", this.isViewMemberInfo);
        }

        #endregion ISerializable Members
    }
}