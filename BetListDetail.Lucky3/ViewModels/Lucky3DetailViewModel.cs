using Fanex.BetListDetail.Lucky3.Enums;
using Fanex.BetListDetail.Lucky3.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.BetListDetail.Lucky3.ViewModels
{
    public class Lucky3DetailViewModel
    {
        public IEnumerable<Lucky3DetailModel> Details { get; set; }

        public decimal? TotalWinlost
        {
            get
            {
                return Details.Sum(i => i.Winlost);
            }
        }

        public decimal TotalSuperDiscount
        {
            get
            {
                return Details.Sum(i => i.SuperDiscount);
            }
        }

        public decimal TotalMasterDiscount
        {
            get
            {
                return Details.Sum(i => i.Discount);
            }
        }

        public decimal TotalAgentDiscount
        {
            get
            {
                return Details.Sum(i => i.AgentDiscount);
            }
        }

        public int TotalColumns
        {
            get
            {
                var staticColumns = 7;
                staticColumns += IsSuper ? 3 : 0;
                staticColumns += IsMaster ? 2 : 0;
                staticColumns += IsAgent ? 1 : 0;

                return staticColumns;
            }
        }

        public int UserLevel { protected get; set; }

        public bool IsSuper
        {
            get
            {
                return UserLevel == (int)UserTypes.Super;
            }
        }

        public bool IsMaster
        {
            get
            {
                return UserLevel == (int)UserTypes.Master;
            }
        }

        public bool IsAgent
        {
            get
            {
                return UserLevel == (int)UserTypes.Agent;
            }
        }
    }
}