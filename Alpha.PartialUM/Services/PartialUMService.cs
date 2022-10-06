namespace Alpha.PartialUM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.PartialUM.Configurations;
    using Alpha.PartialUM.DataAccess;
    using Alpha.PartialUM.Exceptions;
    using Alpha.PartialUM.Helpers;
    using Alpha.PartialUM.Models;
    using Fanex.Caching;

    public interface IPartialUMService
    {
        void ThrowIfUM(string settingKey);
    }

    public class PartialUMService : IPartialUMService
    {
        private readonly IPartialUMDataAccess partialUMDataAccess;

        public PartialUMService(IPartialUMDataAccess partialUMDataAccess)
            => this.partialUMDataAccess = partialUMDataAccess;

        public void ThrowIfUM(string settingKey)
        {
            var functionNames = PartialUMSectionHelper.GetFunctionNames(settingKey);
            if (!functionNames.Any())
            {
                return;
            }

            var umInfoDictionary = GetUMInfoDictionary();
            if (umInfoDictionary.Count == 0)
            {
                return;
            }

            foreach (var function in functionNames)
            {
                if (umInfoDictionary.TryGetValue(function, out var umInfo))
                {
                    throw new PartialUMException(FormatUMTime(umInfo.BeginTime), FormatUMTime(umInfo.EndTime));
                }
            }
        }

        private IDictionary<string, UMInfo> GetUMInfoDictionary()
            => ConfiguredCacheService
                .GetOrSet(
                $"Alpha.PartialUM.Services.MinimizedUM.{AppSettings.SiteGroup}",
                () => GetUMInfoDictionaryFromDB(AppSettings.SiteGroup),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(AppSettings.PartialUMReportsBetListsCacheInSeconds)));

        private IDictionary<string, UMInfo> GetUMInfoDictionaryFromDB(string siteGroup)
        {
            var umInfoList = partialUMDataAccess.GetUMInfoList(siteGroup);
            var umInfoDictionary = new Dictionary<string, UMInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in umInfoList)
            {
                umInfoDictionary[item.FunctionTypeName] = item;
            }

            return umInfoDictionary;
        }

        private static string FormatUMTime(DateTime dateTime)
            => dateTime.ToString("dd/MM/yyyy hh:mm tt");
    }
}
