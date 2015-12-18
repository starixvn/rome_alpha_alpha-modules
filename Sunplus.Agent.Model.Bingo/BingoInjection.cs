namespace Sunplus.Agent.Model.BL.Bingo
{
    using System;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Libraries.ThirdPartyTestData;

    [Obsolete("Should rename to Sunplus.Agent.Model.Bingo")]
    internal static class BingoInjection
    {
        static BingoInjection()
        {
            BingoAPICaller bingoApiCaller = new BingoAPICaller();
            APICallerManager apiCallerManager = new APICallerManager(bingoApiCaller);
            apiCallerManager.Register(typeof(LiveCasinoLibrary.SBAgent.Report), "WinLoss*|RealTime*");
        }

        public static void Init()
        {
        }

        private static int GetCustId()
        {
            return UserSession.Current.CustId;
        }

        private static int GetUserId()
        {
            return UserSession.Current.UserId;
        }
    }
}
