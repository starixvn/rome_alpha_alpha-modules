namespace Alpha.RemotingRef
{
    public interface CacheRefService
    {
        string GetBetType(object betTypeId);

        string GetBetType(object betTypeId, object betId);

        string GetBetTypeName(object betTypeId);

        string GetSportName(object sportId);

        string GetTeamName(object teamId);

        string GetRouletteBetItem(object itemId);

        string GetCasinoBetItem(object betStatusId);

        string GetLeagueName(object leagueId);

        string GetFncIndexName(object indexId);

        string GetHorseTeamName(object codeId);

        string GetOddsTypeName(object oddsTypeId);

        string GetProductName(int productId);
    }
}
