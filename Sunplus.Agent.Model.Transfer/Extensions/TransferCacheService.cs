using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Web.Configuration;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Sunplus.Agent.Model.BL.Definitions;
using Sunplus.Agent.Model.Transfer.Models;

namespace Sunplus.Agent.Model.Transfer.Extensions
{
    public interface TransferCacheService
    {
        List<TransferCacheModel> Get(string cacheKey);

        bool Set(string cacheKey, List<TransferCacheModel> value);

        void Remove(string cacheKey);

        TransferCacheModel CacheContains(List<TransferCacheModel> listTransferCacheModel, int custId);

        TransferCacheModel CacheContainsExcludeSelf(List<TransferCacheModel> listTransferCacheModel, int selfAdminId, int custId);

        void CacheAdd(List<TransferCacheModel> listTransferCacheModel, int custId, int adminId, string adminName);

        void CacheRemove(List<TransferCacheModel> listTransferCacheModel, int custId);

        void CacheRemoveSuccessTransferCustIds(string cacheKey, List<int> listSuccessTransferCustIds);
    }

    public class TransferCacheServiceImplement : TransferCacheService
    {
        private int _adminId;
        private string _adminName = "Agent";
        private readonly ICacheService cacheService;

        public TransferCacheServiceImplement() : this(0, string.Empty, null)
        { }

        public TransferCacheServiceImplement(int adminId, string adminName, ICacheService cacheService)
        {
            _adminId = adminId;
            _adminName = adminName;
            this.cacheService = cacheService ?? InitCacheService();
        }

        private static ICacheService InitCacheService()
        {
            var redisConfigurationOptions = WebConfigurationManager.AppSettings["RedisConfigurationOptions:Alpha.Transfer"];
            var configurationOptions = ConfigurationOptions.Parse(redisConfigurationOptions);
            var redisCacheOptions = new RedisCacheOptions
            {
                ApplicationName = configurationOptions.ClientName,
                ConnectionOptions = configurationOptions
            };

            return new CacheService(new RedisCache(redisCacheOptions));
        }

        public List<TransferCacheModel> Get(string cacheKey)
        {
            string value = cacheService.Get<string>(cacheKey);

            return string.IsNullOrWhiteSpace(value)
                ? null
                : JsonConvert.DeserializeObject<List<TransferCacheModel>>(value);
        }

        public bool Set(string cacheKey, List<TransferCacheModel> value)
        {
            var cachePolicy = new CacheItemOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(AppSettings.MultipleTransferCacheTimeOut));
            cacheService.Set(cacheKey, JsonConvert.SerializeObject(value), cachePolicy);

            if (AppSettings.EnableTransferLogInfo)
            {
                LogService.Info(InfoMessageFormat(value), AppSettings.TransferLogCategory);
            }

            if (!CompareAfterSet(cacheKey, value))
            {
                LogService.Error(ErrorMessageFormat(value));
                return false;
            }

            return true;
        }

        public void Remove(string cacheKey)
        {
            cacheService.Remove(cacheKey);
        }

        public TransferCacheModel CacheContains(List<TransferCacheModel> listTransferCacheModel, int custId)
        {
            foreach (var item in listTransferCacheModel)
            {
                if (item.CustIds.Contains(0) || item.CustIds.Contains(custId))
                {
                    return item;
                }
            }

            return null;
        }

        public TransferCacheModel CacheContainsExcludeSelf(List<TransferCacheModel> listTransferCacheModel, int selfAdminId, int custId)
        {
            foreach (var item in listTransferCacheModel)
            {
                if (item.AdminId != selfAdminId && (item.CustIds.Contains(0) || item.CustIds.Contains(custId)))
                {
                    return item;
                }
            }

            return null;
        }

        public void CacheAdd(List<TransferCacheModel> listTransferCacheModel, int custId, int adminId, string adminName)
        {
            foreach (var item in listTransferCacheModel)
            {
                if (item.AdminId == adminId)
                {
                    item.CustIds.Add(custId);
                    return;
                }
            }

            var newAdminTransferCache = new TransferCacheModel()
            {
                AdminId = adminId,
                AdminName = adminName
            };

            newAdminTransferCache.CustIds.Add(custId);
            listTransferCacheModel.Add(newAdminTransferCache);
        }

        public void CacheRemove(List<TransferCacheModel> listTransferCacheModel, int custId)
        {
            foreach (var cacheModel in listTransferCacheModel)
            {
                if (cacheModel.CustIds.Contains(custId))
                {
                    cacheModel.CustIds.Remove(custId);
                    return;
                }
            }
        }

        public void CacheRemoveSuccessTransferCustIds(string cacheKey, List<int> listSuccessTransferCustIds)
        {
            var listMemberTransferCache = Get(cacheKey);
            if (listMemberTransferCache != null)
            {
                foreach (var successTransferCustId in listSuccessTransferCustIds)
                {
                    CacheRemove(listMemberTransferCache, successTransferCustId);
                }

                Set(cacheKey, listMemberTransferCache);
            }
        }

        private bool CompareAfterSet(string cacheKey, List<TransferCacheModel> value)
        {
            var temp = this.Get(cacheKey);
            if (temp == null && value == null)
            {
                return true;
            }

            if (temp != null && value != null && value.Count == temp.Count)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (!value[i].Equals(temp[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private string ErrorMessageFormat(List<TransferCacheModel> value)
            => $"[Agent Transfer Process] Redis cannot set value: {JsonConvert.SerializeObject(value)}";

        private string InfoMessageFormat(List<TransferCacheModel> value)
            => $"[Agent Transfer Process - PID: {Thread.CurrentThread.ManagedThreadId}] {_adminName}({_adminId}) set redis value to: {JsonConvert.SerializeObject(value)}";
    }
}
