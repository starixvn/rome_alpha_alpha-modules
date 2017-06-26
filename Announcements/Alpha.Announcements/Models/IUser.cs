namespace Alpha.Announcements.Models
{
    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string Language { get; }

        byte LevelId { get; }

        int AdminId { get; }

        bool IsInternal { get; }

        string LangKey { get; }

        int CurrencyId { get; }

        int ARecommendId { get; }

        string Site { get; }

        bool IsSubAcc { get; }
    }
}