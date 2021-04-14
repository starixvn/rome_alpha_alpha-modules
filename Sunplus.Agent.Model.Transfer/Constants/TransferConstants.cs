using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Agent.Model.Transfer.Constants
{
    public class TransferConstants
    {
        public const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";

        public const string SuccessTransferResultMessage = "Successful transfer(s): {0}<br/>";

        public const string ConcurrentTransferResultMessage = "<span style=\"color: red\">Transfer(s) has been proceeding by {0}:</span> {1}<br/>";

        public const string ConcurrentSingleTransferResultMessage = "<span style=\"color: red\">The transfer has been proceeding by {0}<br/>";

        public const string CacheErrorMessage = "Agent Transfer Process Error Log.\r\nThere is some error with Cache: {0}\r\nInner Message: {1}.";
    }
}
