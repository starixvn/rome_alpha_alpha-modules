using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Agent.Model.Transfer.Models
{
    public class MultipleTransferMemberRequest
    {
        public int Status { get; set; }

        public string UserName { get; set; }

        public bool IsYesterdayBalance { get; set; }

        public string CustIds { get; set; }

        public string IsClose { get; set; }

        public bool CheckAll { get; set; }

        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public string ClientIP { get; set; }

        public string NetSessionId { get; set; }

        public string TransferSuccessMessage { get; set; }
    }
}
