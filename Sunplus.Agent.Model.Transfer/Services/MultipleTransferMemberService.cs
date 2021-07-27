using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Fanex.Caching;
using Fanex.Logging;
using Sunplus.Agent.Model.BL.Definitions;
using Sunplus.Agent.Model.BL.Transfer;
using Sunplus.Agent.Model.Transfer.Constants;
using Sunplus.Agent.Model.Transfer.Extensions;
using Sunplus.Agent.Model.Transfer.Models;

namespace Sunplus.Agent.Model.Transfer.Services
{
    public class MultipleTransferMemberService
    {
        private MultipleTransferMemberRequest request;

        private TransferStatus transferStatus;

        private TransferResult result;

        private TransferBO bo;

        private DataTable dataTransfer;

        private TransferCacheService transferCacheService;

        private string cacheKey;

        public MultipleTransferMemberService(MultipleTransferMemberRequest multipleTransferMemberRequest, ICacheService cacheService)
        {
            request = multipleTransferMemberRequest;
            result = new TransferResult();
            bo = new TransferBO(request.NetSessionId);
            dataTransfer = bo.GetDataTransfer(request.Status, request.UserName, request.IsYesterdayBalance);
            transferStatus = new TransferStatus();
            request.IsClose = GetStatus();
            transferCacheService = new TransferCacheServiceImplement(multipleTransferMemberRequest.AdminId, multipleTransferMemberRequest.AdminName, cacheService);
            cacheKey = string.Format(TransferConstants.AgentTransferMemberListCache, Alpha.Web.Configurations.AppSettings.SiteGroup, request.AgentId);
        }

        public TransferResult ProcessTransfer()
        {
            if (request.CheckAll && !string.IsNullOrEmpty(request.UserName))
            {
                if (dataTransfer.Rows.Count > 0)
                {
                    StringBuilder custIdsBuilder = new StringBuilder();
                    for (int i = 0; i < dataTransfer.Rows.Count; i++)
                    {
                        custIdsBuilder.Append(dataTransfer.Rows[i]["custid"] + ",");
                    }

                    request.CustIds = custIdsBuilder.ToString().Trim(',');
                }

                request.CheckAll = false;
            }


            foreach (var batch in CreateCustIdBatches())
            {
                if (result.ErrCode != 0)
                {
                    break;
                }

                ProcessTransfer(batch);
            }

            ComposeResultMessage();
            return result;
        }

        private void ProcessTransfer(string _CustID)
        {
            List<int> listCustId = new List<int>(Array.ConvertAll(_CustID.Split(','), int.Parse));
            List<int> listValidCustId = new List<int>(), listInvalidCustId = new List<int>();
            bool isUsingCache = CacheFilterInputListCustIds(listCustId, listValidCustId, listInvalidCustId);
            if (listValidCustId.Count > 0)
            {
                try
                {
                    string errMsg = string.Empty;
                    result.ErrCode += bo.TransferYesCleareBalanceForAgent(isUsingCache ? string.Join(",", listValidCustId) : _CustID, request.CheckAll, request.IsClose, request.ClientIP, out errMsg);
                    result.ErrMsg = errMsg;
                }
                finally
                {
                    if (isUsingCache)
                    {
                        transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, listValidCustId);
                    }
                }
            }

            if (result.ErrCode == 0)
            {
                transferStatus.listCustIdsTransferSuccess.AddRange(isUsingCache ? listValidCustId : listCustId);
            }
            else
            {
                transferStatus.listCustIdsTransferError.Add(new TransferFailedModel() { ListCustIds = isUsingCache ? listValidCustId : listCustId, ErrMsg = result.ErrMsg });
            }
        }

        private List<string> CreateCustIdBatches()
        {
            var custIdsBatches = new List<string>();
            int maxCustidTransferPerBatch = Convert.ToInt16(AppSettings.GetValue("MaxCustidTransfer"));
            int arrCustidLength = request.CustIds.Length;
            if (arrCustidLength > 0)
            {
                int commaCounter = 0;
                int startIndex = 0;
                for (int i = 0; i < arrCustidLength; i++)
                {
                    if (request.CustIds[i] == ',')
                    {
                        commaCounter++;
                    }

                    if (commaCounter == maxCustidTransferPerBatch)
                    {
                        custIdsBatches.Add(request.CustIds.Substring(startIndex, i));
                        startIndex = i + 1;
                        commaCounter = 0;
                    }
                }

                if (commaCounter > 0 || startIndex < arrCustidLength)
                {
                    custIdsBatches.Add(request.CustIds.Substring(startIndex));
                }
            }

            return custIdsBatches;
        }

        private string GetStatus()
        {
            string _status = string.Empty;
            switch (request.Status)
            {
                case 0:
                    _status = "All";
                    break;
                case 1:
                    _status = "Open";
                    break;
                case 2:
                    _status = "Suspended";
                    break;
                case 3:
                    _status = "Closed";
                    break;
                default:
                    _status = "Disabled";
                    break;
            }

            return _status;
        }

        private void ComposeResultMessage()
        {
            string resultMessage = string.Empty;
            string spliter = ", ";
            if (result.ErrCode != 0)
            {
                return;
            }

            if (result.ErrCode == 0 && request.CustIds.Equals("0"))
            {
                result.ErrMsg = transferStatus.listCustIdsTransferConcurrent.Count > 0 ? string.Format(TransferConstants.ConcurrentTransferResultMessage, transferStatus.listCustIdsTransferConcurrent[0].AdminName, "All") : request.TransferSuccessMessage;
                return;
            }

            //if (transferStatus.listCustIdsTransferConcurrent.Count == 0 && transferStatus.listCustIdsTransferError.Count == 0)
            //{
            //    return Resources.Lang.transfersuccessful;
            //}

            //if (transferStatus.listCustIdsTransferError.Count > 0)
            //{
            //    foreach (var custIdsTransferError in transferStatus.listCustIdsTransferError)
            //    {
            //        resultMessage += custIdsTransferError.ErrMsg + "/n";
            //    }

            //    return resultMessage;
            //}

            if (transferStatus.listCustIdsTransferSuccess.Count > 0)
            {
                resultMessage += string.Format(TransferConstants.SuccessTransferResultMessage, string.Join(spliter, GetCustNameFromCustId(transferStatus.listCustIdsTransferSuccess)));
            }

            if (transferStatus.listCustIdsTransferConcurrent.Count > 0)
            {
                StringBuilder concurrentBuilder = new StringBuilder();
                foreach (var custIdsTransferConcurrent in transferStatus.listCustIdsTransferConcurrent)
                {
                    concurrentBuilder.Append(string.Format(TransferConstants.ConcurrentTransferResultMessage, custIdsTransferConcurrent.AdminName, string.Join(spliter, GetCustNameFromCustId(custIdsTransferConcurrent.CustIds))));
                }

                resultMessage += concurrentBuilder.ToString();
            }

            result.ErrMsg = resultMessage;
        }

        private string GetCacheErrorMessage(Exception ex)
        {
            return string.Format(TransferConstants.CacheErrorMessage, ex.Message, ex.InnerException);
        }

        private bool CacheFilterInputListCustIds(List<int> listCustId, List<int> listValidCustId, List<int> listInvalidCustId)
        {
            try
            {
                var listMemberTransferCache = transferCacheService.Get(cacheKey);
                if (listMemberTransferCache != null && listMemberTransferCache.Count > 0)
                {
                    foreach (int custid in listCustId)
                    {
                        var adminsTransferAll = transferCacheService.CacheContains(listMemberTransferCache, 0);
                        var adminsTransferConcurrent = transferCacheService.CacheContains(listMemberTransferCache, custid);
                        if (adminsTransferAll != null)
                        {
                            listInvalidCustId.Add(custid);
                            transferCacheService.CacheAdd(transferStatus.listCustIdsTransferConcurrent, custid, adminsTransferAll.AdminId, adminsTransferAll.AdminName);
                        }
                        else if (adminsTransferConcurrent != null)
                        {
                            listInvalidCustId.Add(custid);
                            transferCacheService.CacheAdd(transferStatus.listCustIdsTransferConcurrent, custid, adminsTransferConcurrent.AdminId, adminsTransferConcurrent.AdminName);
                        }
                        else
                        {
                            transferCacheService.CacheAdd(listMemberTransferCache, custid, request.AdminId, request.AdminName);
                            listValidCustId.Add(custid);
                        }
                    }
                }
                else
                {
                    listMemberTransferCache = new List<TransferCacheModel>();
                    foreach (int custid in listCustId)
                    {
                        transferCacheService.CacheAdd(listMemberTransferCache, custid, request.AdminId, request.AdminName);
                        listValidCustId.Add(custid);
                    }
                }

                transferCacheService.Set(cacheKey, listMemberTransferCache);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(this.GetCacheErrorMessage(ex), ex);
                return false;
            }
        }

        private List<string> GetCustNameFromCustId(List<int> listCustIds)
        {
            var custNames = new List<string>();
            if (dataTransfer.Rows.Count > 0)
            {
                for (int i = 0; i < dataTransfer.Rows.Count; i++)
                {
                    var custId = Convert.ToInt32(dataTransfer.Rows[i]["custid"]);
                    if (listCustIds.Contains(custId))
                    {
                        custNames.Add(Convert.ToString(dataTransfer.Rows[i]["username"]));
                    }
                }
            }
            return custNames;
        }
    }
}
