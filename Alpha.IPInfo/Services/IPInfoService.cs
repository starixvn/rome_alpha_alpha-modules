using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.IPInfo.DataAccess;
using Alpha.IPInfo.Extensions;
using Alpha.IPInfo.Models;
using Fanex.Caching;

namespace Alpha.IPInfo.Services
{
    public interface IIPInfoService
    {
        Task<IDictionary<string, string>> GetCountries(IEnumerable<string> ipList);
    }

    public class IPInfoService : IIPInfoService
    {
        private const string IPV4TopCountriesCacheKey = "Alpha.IPInfo.IPV4.TopCountries";
        private const string IPV4CacheKey = "Alpha.IPInfo.IPV4";
        private const string IPV6CacheKey = "Alpha.IPInfo.IPV6";

        private readonly IPInformationConfig cacheConfig;
        private readonly ICacheService cacheService;
        private readonly IIPInfoDataAccess ipInfoDataAccess;

        public IPInfoService(
            IPInformationConfig cacheConfig,
            ICacheService cacheService,
            IIPInfoDataAccess ipInfoDataAccess)
        {
            this.cacheConfig = cacheConfig;
            this.cacheService = cacheService;
            this.ipInfoDataAccess = ipInfoDataAccess;
        }

        public async Task<IDictionary<string, string>> GetCountries(IEnumerable<string> ipList)
        {
            var ipDictionary = new Dictionary<string, string>();
            var (ipV4List, ipV6List) = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

            if (ipV4List.Count > 0)
            {
                await IPV4ToCountry(ipDictionary, ipV4List);
            }

            if (ipV6List.Count > 0)
            {
                await IPV6ToCountry(ipDictionary, ipV6List);
            }

            return ipDictionary;
        }

        private async Task IPV4ToCountry(IDictionary<string, string> ipDictionary, IList<string> ipList)
        {
            var ipV4TopCountries = await GetIPV4ByTopCountry();
            var ipV4NotInTopCountries = await cacheService.GetAsync<List<IPRange>>(IPV4CacheKey) ?? new List<IPRange>();
            var ipV4NotFoundList = new List<string>();

            foreach (var ip in ipList)
            {
                var ipNumber = IPInfoHelper.ConvertIPV4ToNumber(ip);
                var ipFound = ipV4TopCountries.BinarySearch2(ipNumber) ?? ipV4NotInTopCountries.BinarySearch2(ipNumber);

                if (ipFound == null)
                {
                    ipV4NotFoundList.Add(ip);
                }
                else
                {
                    ipDictionary[ip] = ipFound.Country;
                }
            }

            if (ipV4NotFoundList.Count > 0)
            {
                await GetIPV4Country(ipDictionary, ipV4NotFoundList);
            }
        }

        private Task<IList<IPRange>> GetIPV4ByTopCountry()
            => cacheService.GetOrSetAsync(
                IPV4TopCountriesCacheKey,
                GetIPV4ByTopCountryFromDB,
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheConfig.IPV4TopCountriesCacheInMinutes)));

        private async Task<IList<IPRange>> GetIPV4ByTopCountryFromDB()
        {
            var ipRanges = (await ipInfoDataAccess.GetIPV4InfoByTopCountries(cacheConfig.IPV4TopCountries))
                .Select(ip => new IPRange(ip.IPFrom, ip.IPTo, ip.CountryLong))
                .ToList();
            ipRanges.Sort();

            return ipRanges;
        }

        private async Task GetIPV4Country(IDictionary<string, string> ipDictionary, IList<string> ipList)
        {
            var availableIPV4List = await MergeIPV4Country(ipList);

            foreach (var ip in ipList)
            {
                var ipNumber = IPInfoHelper.ConvertIPV4ToNumber(ip);
                var ipFound = availableIPV4List.BinarySearch2(ipNumber);

                if (ipFound != null)
                {
                    ipDictionary[ip] = ipFound.Country;
                }
            }
        }

        private async Task<IList<IPRange>> MergeIPV4Country(IList<string> ipList)
        {
            var availableIPV4List = await cacheService.GetAsync<List<IPRange>>(IPV4CacheKey) ?? new List<IPRange>();
            availableIPV4List.AddRange(await GetIPV4CountryFromDB(ipList));

            var comparer = new IPRangeEqualityComparer();
            availableIPV4List = availableIPV4List.Distinct(comparer).ToList();
            availableIPV4List.Sort();

            await cacheService.SetAsync(
                IPV4CacheKey,
                availableIPV4List,
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheConfig.IPV4CacheInMinutes)));

            return availableIPV4List;
        }

        private async Task<IEnumerable<IPRange>> GetIPV4CountryFromDB(IList<string> ipList)
            => (await ipInfoDataAccess.GetIPV4Info(ipList.JoinComma()))
                .Select(ip => new IPRange(ip.IPFrom, ip.IPTo, ip.CountryLong));

        private async Task IPV6ToCountry(IDictionary<string, string> ipDictionary, IList<string> ipList)
        {
            var ipNotFoundList = new List<string>();

            foreach (var ip in ipList)
            {
                var country = await cacheService.GetAsync<string>($"{IPV6CacheKey}_{ip}");

                if (country == null)
                {
                    ipNotFoundList.Add(ip);
                }
                else
                {
                    ipDictionary[ip] = country;
                }
            }

            if (ipNotFoundList.Count > 0)
            {
                await GetIPV6Country(ipDictionary, ipNotFoundList);
            }
        }

        private async Task GetIPV6Country(IDictionary<string, string> ipDictionary, IList<string> ipList)
        {
            var ipV6Info = await ipInfoDataAccess.GetIPV6Info(ipList.JoinComma());

            foreach (var ip in ipV6Info)
            {
                await cacheService.SetAsync(
                    $"{IPV6CacheKey}_{ip.IP}",
                    ip.CountryLong,
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheConfig.IPV6CacheInMinutes)));
                ipDictionary[ip.IP] = ip.CountryLong;
            }
        }
    }
}