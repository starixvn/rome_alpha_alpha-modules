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
    using Sunplus.Agent.Model.Entities;

    public interface IPartialUMService
    {
        void ThrowIfUM(string settingKey);

        void ThrowIfUM(string settingKey, string siteName);
    }

    public class PartialUMService : IPartialUMService
    {
        private readonly IPartialUMDataAccess partialUMDataAccess;

        public PartialUMService(IPartialUMDataAccess partialUMDataAccess)
            => this.partialUMDataAccess = partialUMDataAccess;

        public void ThrowIfUM(string settingKey)
            => ThrowIfUM(settingKey, UserSession.Current.Site);

        public void ThrowIfUM(string settingKey, string siteName)
        {
            var functionNames = PartialUMSectionHelper.GetFunctionNames(settingKey);
            if (!functionNames.Any())
            {
                return;
            }

            var umInfoDictionary = GetUMInfoDictionary(siteName);
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

        private IDictionary<string, UMInfo> GetUMInfoDictionary(string siteName)
            => ConfiguredCacheService
                .GetOrSet(
                $"Alpha.PartialUM.Services.MinimizedUM.SiteName-{siteName}",
                () => GetUMInfoDictionaryFromDB(siteName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(AppSettings.PartialUMReportsBetListsCacheInSeconds)));

        private IDictionary<string, UMInfo> GetUMInfoDictionaryFromDB(string siteName)
        {
            var umInfoList = partialUMDataAccess.GetUMInfoList(siteName);
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
