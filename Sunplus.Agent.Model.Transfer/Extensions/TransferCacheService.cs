using Fanex.Logging;
using Newtonsoft.Json;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.Transfer.Models;
using Sunplus.Agent.Utilities.Extension;
using System.Collections.Generic;

namespace Sunplus.Agent.Model.Transfer.Extensions
{
    public interface TransferCacheService
    {
        List<TransferCacheModel> Get(string cacheKey);

        bool Set(string cacheKey, List<TransferCacheModel> value);

        void Remove(string cacheKey);

        TransferCacheModel CacheContains(List<TransferCacheModel> listTransferCacheModel, int custId);

        void CacheAdd(List<TransferCacheModel> listTransferCacheModel, int custId, int adminId, string adminName);

        void CacheRemove(List<TransferCacheModel> listTransferCacheModel, int custId);

        void CacheRemoveSuccessTransferCustIds(string cacheKey, List<int> listSuccessTransferCustIds);
    }

    public class TransferCacheServiceImplement : TransferCacheService
    {
        public List<TransferCacheModel> Get(string cacheKey)
        {
            return CacheExtension.Get<List<TransferCacheModel>>(cacheKey);
        }

        public bool Set(string cacheKey, List<TransferCacheModel> value)
        {
            CacheExtension.Set(cacheKey, value);
            if (!CompareAfterSet(cacheKey, value))
            {
                Logger.Log.Error(ErrorMessageFormat(value));
                return false;
            }

            return true;
        }

        public void Remove(string cacheKey)
        {
            CacheExtension.Remove(cacheKey);
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
            var temp = CacheExtension.Get<List<TransferCacheModel>>(cacheKey);
            if (temp == null)
            {
                return value == null;
            }

            if (value == null)
            {
                return temp == null;
            }

            if (value.Count != temp.Count)
            {
                return false;
            }

            for (int i = 0; i < value.Count; i++)
            {
                if (!value[i].Equals(temp[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private string ErrorMessageFormat(List<TransferCacheModel> value)
        {
            string msg = "[Agent Transfer Process] Couchbase cannot set value: ";
            msg += JsonConvert.SerializeObject(value);
            return msg;
        }


    }
}
