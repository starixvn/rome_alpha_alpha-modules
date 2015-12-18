namespace Sunplus.Agent.Model.Common
{
    using System;

    [Obsolete("Should change namespace to Sunplus.Agent.Utilities")]
    public static class Language
    {
        private static readonly string[] Languages = new string[] { "en-US", "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR", "vi-VN" };

        // E,J,CT,CS,TH,KO.
        private static readonly string[] LanguagesKeys = new string[] { "E", "CT", "J", "TH", "CS", "KO", "VI" };

        /// <summary>
        /// Check is valid language string.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool IsValidLanguage(string culture)
        {
            int c = Languages.Length;
            for (int i = 0; i < c; i++)
            {
                if (Languages[i] == culture)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get language key from culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string LangKeyFromCulture(string culture)
        {
            int c = Languages.Length;
            for (int i = 0; i < c; i++)
            {
                if (Languages[i] == culture)
                {
                    return LanguagesKeys[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Get culture from language key.
        /// </summary>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public static string CultureFromLangKey(string langKey)
        {
            int c = LanguagesKeys.Length;
            for (int i = 0; i < c; i++)
            {
                if (LanguagesKeys[i] == langKey)
                {
                    return Languages[i];
                }
            }

            return null;
        }
    }
}