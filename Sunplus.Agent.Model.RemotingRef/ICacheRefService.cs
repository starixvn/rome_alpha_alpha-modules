namespace Sunplus.Agent.Model.RemotingRef
{
    public interface ICacheRefService
    {
        string GetBettype(object bettypeId);

        string GetBettype(object bettypeId, object betId);

        string GetBetTypeName(object bettypeId);

        string GetSportName(object sportId);

        string GetTeamName(object teamId);

        string GetRouletteBetItem(object itemId);

        string GetCasinoBetItem(object betStatusId);

        string GetLeagueName(object leagueId);

        string GetFncIndexName(object indexId);

        string GetHorseTeamName(object codeId);

        string GetOddsTypeName(object oddsTypeId);
    }
}