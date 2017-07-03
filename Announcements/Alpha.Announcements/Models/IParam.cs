using System.Collections.Generic;

namespace Alpha.Announcements.Models
{
    public interface IParam
    {
        int Id { get; }

        string Name { get; }

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