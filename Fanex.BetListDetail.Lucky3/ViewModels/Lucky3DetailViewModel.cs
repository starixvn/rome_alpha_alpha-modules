namespace Fanex.BetListDetail.Lucky3.ViewModels
{
    using Fanex.BetListDetail.Lucky3.Constants;
    using Fanex.BetListDetail.Lucky3.Enums;
    using Fanex.BetListDetail.Lucky3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class Lucky3DetailViewModel
    {
        public Lucky3DetailViewModel(IEnumerable<Lucky3DetailModel> details, int userLevel)
        {
            this.Details = details;
            this.UserLevel = userLevel;
            CalculateTotal();
        }

        public IEnumerable<Lucky3DetailModel> Details { get; }

        public decimal TotalWinlost { get; private set; }

        public decimal TotalSuperCommission { get; private set; }

        public decimal TotalMasterCommission { get; private set; }

        public decimal TotalAgentCommission { get; private set; }

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

        public int UserLevel { get; }

        public bool IsSuper => UserLevel == (int)UserTypes.Super;

        public bool IsMaster => UserLevel == (int)UserTypes.Master;

        public bool IsAgent => UserLevel == (int)UserTypes.Agent;

        public int FormatBets(decimal bets) => (int)bets;

        public string FormatDiscount(decimal discount) => string.Format(Constant.DiscountTemplate, discount * 100m);

        public string FormatCommission(decimal commission) => string.Format(Constant.CommissionTemplate, commission);

        public string FormatMoney(decimal stake) => string.Format(Constant.MoneyTemplate, stake);

        public string FormatWinLost(decimal? winlost)
        {
            if (!winlost.HasValue)
            {
                return string.Empty;
            }

            var text = FormatMoney(winlost.Value);
            var lessThanZero = winlost.Value < 0;
            if (lessThanZero)
            {
                text = string.Format(Constant.NegativeNumberDisplay, text);
            }

            return text;
        }

        private void CalculateTotal()
        {
            TotalWinlost = 0;
            TotalSuperCommission = 0;
            TotalMasterCommission = 0;
            TotalAgentCommission = 0;
            foreach (var detail in Details)
            {
                TotalWinlost += detail.Winlost.HasValue ? detail.Winlost.Value : 0;
                TotalSuperCommission += detail.SuperCommission;
                TotalMasterCommission += detail.MasterCommission;
                TotalAgentCommission += detail.AgentCommission;
            }
        }
    }
}