using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Agent.Model.Transfer.Models
{
    [Serializable]
    public class TransferCacheModel
    {
        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public List<int> CustIds { get; set; }

        public bool Equals(TransferCacheModel obj)
        {
            if (AdminId != obj.AdminId)
            {
                return false;
            }

            for (int i = 0; i < CustIds.Count; i++)
            {
                if (CustIds[i] != obj.CustIds[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
