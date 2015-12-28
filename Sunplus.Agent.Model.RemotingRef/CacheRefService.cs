namespace Sunplus.Agent.Model.RemotingRef
{
    using BL.Cache;

    public class CacheRefService : ICacheRefService
    {
        public string GetBettype(object bettypeId)
        {
            return CacheRef.GetBettype(bettypeId);
        }

        public string GetBettype(object bettypeId, object betId)
        {
            return CacheRef.GetBettype(bettypeId, betId);
        }

        public string GetBetTypeName(object bettypeId)
        {
            return CacheRef.GetBetTypeName(bettypeId);
        }

        public string GetCasinoBetItem(object betStatusId)
        {
            return CacheRef.GetCasinoBetItem(betStatusId);
        }

        public string GetFncIndexName(object indexId)
        {
            return CacheRef.GetFncIndexName(indexId);
        }

        public string GetHorseTeamName(object codeId)
        {
            return CacheRef.GetHorseTeamName(codeId);
        }

        public string GetLeagueName(object leagueId)
        {
            return CacheRef.GetLeagueName(leagueId);
        }

        public string GetOddsTypeName(object oddsTypeId)
        {
            return CacheRef.GetOddsTypeName(oddsTypeId);
        }

        public string GetRouletteBetItem(object itemId)
        {
            return CacheRef.GetRouletteBetItem(itemId);
        }

        public string GetSportName(object sportId)
        {
            return CacheRef.GetSportName(sportId);
        }

        public string GetTeamName(object teamId)
        {
            return CacheRef.GetTeamName(teamId);
        }
    }
}