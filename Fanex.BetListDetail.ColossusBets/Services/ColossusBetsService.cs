namespace Fanex.BetListDetail.ColossusBets.Services
{
    using Api;
    using Fanex.BetListDetail.ColossusBets.Configurations;
    using Fanex.BetListDetail.ColossusBets.Constants;
    using Fanex.BetListDetail.ColossusBets.Criteria;
    using Fanex.BetListDetail.ColossusBets.DataAccesses;
    using Fanex.BetListDetail.ColossusBets.Models;
    using Fanex.BetListDetail.ColossusBets.ViewModel;
    using System;
    using System.Web.Script.Serialization;

    public interface ColossusBetsService
    {
        ColossusBetsDetailModel GetBetListDetail(GetColossusBetsBetDetailsParameter parameter);
    }

    public class ColossusBetsServiceImpl : ColossusBetsService
    {
        private readonly ColossusBetsRepository colossusBetsRepository;
        private readonly ColossusBetsWebClient webClient;

        public ColossusBetsServiceImpl(ColossusBetsRepository colossusBetsRepository, ColossusBetsWebClient webClient = null)
        {
            this.colossusBetsRepository = colossusBetsRepository;
            this.webClient = webClient ?? new ColossusBetsWebClientImpl();
        }

        public ColossusBetsDetailModel GetBetListDetail(GetColossusBetsBetDetailsParameter parameter)
        {
            var betDetails = colossusBetsRepository.BetDetails(parameter);
            var memberChoice = GetMemberChoice(parameter);

            return new ColossusBetsDetailModel(memberChoice, betDetails);
        }

        private MemberChoiceResultModel GetMemberChoice(GetColossusBetsBetDetailsParameter parameter)
        {
            var siteId = GetSiteIdByRole(parameter.RoleId);
            var token = GetToken(parameter.AccId, parameter.CustId);
            var memberChoice = string.Format(
                AppSettings.ColossusBetsConfig.MemberChoiceAddress,
                parameter.EventId,
                token.Data.Authorization,
                siteId);

            var domain = GetDomainByCustomerId(parameter.CustId);

            return new MemberChoiceResultModel(domain + memberChoice, token);
        }

        private string GetDomainByCustomerId(int custId)
        {
            var siteId = colossusBetsRepository.GetSiteIdByCustomerId(custId);

            return AppSettings.ColossusBetsConfig.GetDomainBySiteId(siteId);
        }

        private string GetSiteIdByRole(int roleId)
        {
            var siteIds = AppSettings.ColossusBetsConfig.SiteIds.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            switch (roleId)
            {
                case RoleIdConstants.Super:
                    return siteIds[0];

                case RoleIdConstants.Master:
                    return siteIds[1];

                default:
                    return siteIds[2];
            }
        }

        private TokenResultModel GetToken(int userId, int customerId)
        {
            var token = new TokenResultModel { ErrorCode = 1, ErrorMsg = Resources.Resource.ColossusBetsAPI_FailedToConnect };
            try
            {
                var tokenUrl = string.Format(AppSettings.ColossusBetsConfig.TokenAddress, userId, customerId);
                var jsonString = webClient.DownloadString(new Uri(tokenUrl));

                var serializer = new JavaScriptSerializer();
                token = serializer.Deserialize<TokenResultModel>(jsonString);
            }
            catch
            {
                //// exception.Write();
            }

            return token;
        }
    }
}
