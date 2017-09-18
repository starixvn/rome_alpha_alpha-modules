namespace Fanex.BetDetails.Casino.ViewModels
{
    using System.Collections.Generic;
    using BetList.Core.Builder;
    using BetList.Core.Entities;
    using Enums;
    using Models;

    public class CasinoDetailsViewModel
    {
        private static readonly IDictionary<UserLevels, int> TotalColumnByViewLevel = new Dictionary<UserLevels, int>
        {
            { UserLevels.Agent, 8 },
            { UserLevels.Master, 9 },
            { UserLevels.Super, 10 }
        };

        public CasinoDetailsViewModel(
            string sportName,
            int viewerId,
            int viewerLevel,
            PagedData<ITicket> detailData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            ViewerId = viewerId;
            ViewerLevel = (UserLevels)viewerLevel;
            DetailData = detailData;
            Builder = builder;
            TicketHelper = ticketHelper;

            Title = (!string.IsNullOrEmpty(sportName) ? sportName : "RNG Casino") + " Details";
            TotalColumn = TotalColumnByViewLevel[ViewerLevel];
        }

        public string Title { get; }

        public int ViewerId { get; }

        public UserLevels ViewerLevel { get; }

        public PagedData<ITicket> DetailData { get; }

        public IBetListHTMLBuilder Builder { get; }

        public ITicketHelper TicketHelper { get; }

        public bool AllowToViewSuperPT => ViewerLevel == UserLevels.Super;

        public bool AllowToViewMasterPT => ViewerLevel == UserLevels.Master || ViewerLevel == UserLevels.Super;

        public int TotalColumn { get; }

        public PagerViewModel PagerViewModel
            => new PagerViewModel(
                DetailData.CurrentPageIndex,
                DetailData.PageSize,
                DetailData.TotalRecords);
    }
}
