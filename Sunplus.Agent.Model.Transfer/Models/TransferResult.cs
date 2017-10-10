using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Agent.Model.Transfer.Models
{
    public class TransferResult
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public TransferResult()
        {
        }

        public TransferResult(int errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

    }
}
