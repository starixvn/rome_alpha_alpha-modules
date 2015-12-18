namespace Sunplus.Agent.Model.BL.Definitions
{
    using System;

    [Obsolete("Should rename to Sunplus.Agent.Model.Definitions")]
    public enum MatchSettlementStatus : int
    {
        Running = 0,
        Refund = 1,
        RefundHT = 2,
        RefundFT = 3,
        Completed = 4
    }
}