namespace Sunplus.Agent.Model.BL.Casino
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CasinoLibrary.SBAgent;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Libraries.APILogger;

    [Obsolete("Should rename to Sunplus.Agent.Model.Casino")]
    public class CasinoBO : BOBase
    {
        public const int AgentSystem = 2;
        public const string CasinoGroup1 = "01";
        public const string CasinoGroup2 = "02";
        private Customer casinoCustomer = null;
        private Report casinoReport = null;

        public CasinoBO(string netSessionId)
            : base(netSessionId)
        {
            this.casinoCustomer = new Customer();
            this.casinoReport = new Report();

            CasinoInjection.Init();
        }

        public Customer Customer
        {
            get
            {
                return this.casinoCustomer;
            }
        }

        public Report Report
        {
            get
            {
                return this.casinoReport;
            }
        }

        /// <summary>
        /// Return a string which matching with current language
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string CasinoLang(string original)
        {
            switch (Sunplus.Agent.Model.Entities.UserSession.Current.Language)
            {
                case "en-US": return original + "_En";
                case "ja-JP": return original + "_Jp";
                case "zh-TW": return original + "_Ct"; // Traditional
                case "zh-CN": return original + "_Cs"; // Simplified
                case "th-TH": return original + "_Th";
                case "ko-KR": return original + "_Ko";
                default: return original + "_En";
            }
        }

        [Obsolete("Please call via AuthorizationBO.IsSynchronized")]
        public bool IsSynchronized(int custId)
        {
            throw new NotImplementedException("Method was moved to AuthorizationBO class");
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    internal static class CasinoInjection
    {
        static CasinoInjection()
        {
            string pattern = "AutoInsert*|Update*|Insert*|Move*|Reset*|FullSettlementResetMaxTransfer|Rollback*";
            var manager = new APILogManager(new CasinoAPILogger(CasinoBO.AgentSystem, GetUserId));
            manager.Register(typeof(CasinoLibrary.SBAgent.Customer), pattern);
            manager.Register(typeof(CasinoLibrary.SBAgent.BasicInfo), pattern);
        }

        public static void Init()
        {
        }

        private static int GetUserId()
        {
            return UserSession.Current.UserId;
        }
    }
}