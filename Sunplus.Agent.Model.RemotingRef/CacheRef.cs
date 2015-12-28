using RemotingRef;

namespace Sunplus.Agent.Model.BL.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    [Obsolete("Should rename to Sunplus.Agent.Model.RemotingRef")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "Reviewed.")]
    public static class CacheRef
    {
        private const string DefaultLanguage = "en";

        private static readonly Dictionary<string, string> LanguageCodeMappingTable = new Dictionary<string, string>()
            {
                { "en-US", DefaultLanguage },
                { "zh-TW", "ch" },
                { "zh-CN", "cs" },
                { "ja-JP", "jp" },
                { "ko-KR", "ko" },
                { "vi-VN", "vn" }
            };

        private static string RemotingRefLanguageCode
        {
            get
            {
                return LanguageCodeMappingTable.ContainsKey(Thread.CurrentThread.CurrentCulture.Name)
                    ? LanguageCodeMappingTable[Thread.CurrentThread.CurrentCulture.Name]
                    : DefaultLanguage;
            }
        }

        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        public static string GetBettype(object bettypeId, object betId)
        {
            // resourceId remove first character
            string resourceId = string.Empty;
            if (betId != null && betId.ToString().Length > 1)
            {
                resourceId = betId.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(bettypeId.ToString(), RemotingRefLanguageCode, string.Empty, resourceId);
        }

        public static string GetBetTypeName(object bettypeId)
        {
            return bettypeId != null
                ? RefData.GetBetTypeName(bettypeId.ToString(), RemotingRefLanguageCode)
                : string.Empty;
        }

        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), RemotingRefLanguageCode, string.Empty);
        }

        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), RemotingRefLanguageCode, string.Empty);
        }

        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), RemotingRefLanguageCode, string.Empty);
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), RemotingRefLanguageCode, string.Empty);
        }

        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), RemotingRefLanguageCode, string.Empty);
        }

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), RemotingRefLanguageCode);
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), RemotingRefLanguageCode);
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), RemotingRefLanguageCode);
        }
    }
}