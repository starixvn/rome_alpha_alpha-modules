namespace Alpha.RemotingRef
{
    public class CacheRefServiceImpl : CacheRefService
    {
        public string GetBetType(object betTypeId)
        {
            return CacheRef.GetBetTtype(betTypeId);
        }

        public string GetBetType(object betTypeId, object betId)
        {
            return CacheRef.GetBetType(betTypeId, betId);
        }

        public string GetBetTypeName(object betTypeId)
        {
            return CacheRef.GetBetTypeName(betTypeId);
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

        public string GetLeagueName3rd(object leagueId) => CacheRef.GetLeagueName3rd(leagueId);

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

        public string GetTeamName3rd(object teamId) => CacheRef.GetTeamName3rd(teamId);

        public string GetProductName(int productId)
        {
            return CacheRef.GetProductName(productId);
        }
    }
}