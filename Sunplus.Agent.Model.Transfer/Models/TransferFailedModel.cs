using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Agent.Model.Transfer.Models
{
    public class TransferFailedModel
    {
        public List<int> ListCustIds { get; set; }

        public string ErrMsg { get; set; }
    }
}
