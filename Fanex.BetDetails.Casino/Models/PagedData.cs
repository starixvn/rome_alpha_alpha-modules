namespace Fanex.BetDetails.Casino.Models
{
    using System.Collections.Generic;

    public class PagedData<T> where T : class
    {
        public PagedData(
            IEnumerable<T> tickets,
            int currentPageIndex,
            int pageSize,
            int totalRecords)
        {
            Tickets = tickets;
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }

        public IEnumerable<T> Tickets { get; }

        public int CurrentPageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public bool Any() => TotalRecords > 0;

        public bool MoreThanOne() => TotalRecords > 1;
    }
}
