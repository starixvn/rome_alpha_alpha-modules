using RemotingRef;

namespace Alpha.RemotingRef
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

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

        private static string CurrentLanguageCode
        {
            get
            {
                return LanguageCodeMappingTable.ContainsKey(Thread.CurrentThread.CurrentCulture.Name)
                    ? LanguageCodeMappingTable[Thread.CurrentThread.CurrentCulture.Name]
                    : DefaultLanguage;
            }
        }

        public static string GetBetTtype(object betTypeId)
        {
            return GetBetType(betTypeId, null);
        }

        public static string GetBetType(object betTypeId, object betId)
        {
            string resourceId = string.Empty;

            if (betId != null && betId.ToString().Length > 1)
            {
                // resourceId remove first character
                resourceId = betId.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(betTypeId.ToString(), CurrentLanguageCode, string.Empty, resourceId);
        }

        public static string GetBetTypeName(object betTypeId)
        {
            return betTypeId != null
                ? RefData.GetBetTypeName(betTypeId.ToString(), CurrentLanguageCode)
                : string.Empty;
        }

        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), CurrentLanguageCode, string.Empty);
        }

        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), CurrentLanguageCode, string.Empty);
        }

        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), CurrentLanguageCode, string.Empty);
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), CurrentLanguageCode, string.Empty);
        }

        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), CurrentLanguageCode, string.Empty);
        }

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), CurrentLanguageCode);
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), CurrentLanguageCode);
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentLanguageCode);
        }

        public static string GetProductName(int productId)
        {
            return RefData.GetResourceDataByRefIdAndResourceId("Product", productId.ToString(), CurrentLanguageCode, string.Empty);
        }
    }
}