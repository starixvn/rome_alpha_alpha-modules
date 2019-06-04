namespace Fanex.BetDetails.BetBuilder.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public class BetBuilderDetailViewModel
    {
        private readonly IEnumerable<string> SettledStatus = new List<string> { "won", "lose", "refund" };

        public BetBuilderDetailViewModel(IEnumerable<BetBuilderDetailModel> details, string status)
        {
            Details = details;
            Status = status;
        }

        public IEnumerable<BetBuilderDetailModel> Details { get; }

        public bool IsSettled => SettledStatus.Any(i => i.Equals(Status.ToLower()));

        private string Status { get; }
    }
}
