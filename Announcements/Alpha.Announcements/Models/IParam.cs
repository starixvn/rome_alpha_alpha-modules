using System.Collections.Generic;

namespace Alpha.Announcements.Models
{
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

        string Site { get; }

        IEnumerable<byte> OfferedProductIds { get; }

        bool IsSubAcc { get; }

        bool IsSyncedCasino { get; }
    }
}