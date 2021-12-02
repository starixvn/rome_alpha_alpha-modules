namespace Alpha.Announcements.Models
{
    using System.Collections.Generic;

    public interface IParam
    {
        int UserId { get; }

        string UserName { get; }

        string Language { get; }

        byte LevelId { get; }

        int AdminId { get; }

        bool IsInternal { get; }

        string LangKey { get; }

        byte CurrencyId { get; }

        int ARecommendId { get; }

        int MRecommendId { get; }

        int SRecommendId { get; }

        bool IsSuper { get; }

        bool IsMaster { get; }

        bool IsAgent { get; }

        string Site { get; }

        IEnumerable<byte> OfferedProductIds { get; }

        bool IsSubAcc { get; }

        bool EnableYesterdayTransfer { get; }

        Dictionary<string,object> DynamicParams { get; }
    }
}