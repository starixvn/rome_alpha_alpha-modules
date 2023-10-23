namespace Fanex.BetDetails.Casino.ViewModels
{
    using System.Collections.Generic;
    using BetList.Core.Builder;
    using BetList.Core.Entities;
    using Enums;
    using Models;
    using Params;

    public class CasinoDetailsBaseViewModel
    {
        private static readonly IDictionary<UserLevels, int> TotalColumnByViewLevel = new Dictionary<UserLevels, int>
        {
            { UserLevels.Agent, 9 },
            { UserLevels.Master, 10 },
            { UserLevels.Super, 11 }
        };

        public CasinoDetailsBaseViewModel(
            string sportName,
            int viewerId,
            int viewerLevelId,
            CasinoDetailsParam param,
            PagedData<ITicket> detailData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            SportName = sportName;
            ViewerId = viewerId;
            ViewerLevelId = viewerLevelId;

            RefNo = param.RefNo;
            CustId = param.CustId;
            TransType = param.TransType;
            ProductId = param.ProductId;

            DetailData = detailData;
            Builder = builder;
            TicketHelper = ticketHelper;

            Title = (!string.IsNullOrEmpty(sportName) ? sportName : "RNG Casino") + " Details";
            TotalColumn = TotalColumnByViewLevel[ViewerLevel];
        }

        public string Title { get; }

        public long RefNo { get; }

        public int CustId { get; }

        public byte TransType { get; }

        public string SportName { get; }

        public int ProductId { get; }

        public int ViewerId { get; }

        public int ViewerLevelId { get; }

        public UserLevels ViewerLevel => (UserLevels)ViewerLevelId;

        public PagedData<ITicket> DetailData { get; }

        public IBetListHTMLBuilder Builder { get; }

        public ITicketHelper TicketHelper { get; }

        public bool AllowToViewSuperPT => ViewerLevel == UserLevels.Super;

        public bool AllowToViewMasterPT => ViewerLevel == UserLevels.Master || ViewerLevel == UserLevels.Super;

        public int TotalColumn { get; }

        public int StartNo => DetailData.PageSize * (DetailData.CurrentPageIndex - 1) + 1;

        public PagerViewModel PagerViewModel
            => new PagerViewModel(
                DetailData.CurrentPageIndex,
                DetailData.PageSize,
                DetailData.TotalRecords);
    }
}
