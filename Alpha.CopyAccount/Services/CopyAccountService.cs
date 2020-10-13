namespace Alpha.CopyAccount.Services
{
    using Alpha.CopyAccount.Models;
    using Alpha.CopyAccount.Params;
    using RestSharp;
    using System.Threading.Tasks;

    public interface ICopyAccountService
    {
        CopyAccountResult Copy(CopyAccountParams command);

        Task<CopyAccountResult> CopyAsync(CopyAccountParams command);
    }

    public class CopyAccountService : ICopyAccountService
    {
        private const string copyResource = "customer/copy";
        private readonly IRestClient restClient;

        public CopyAccountService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public CopyAccountResult Copy(CopyAccountParams command)
        {
            var request = new RestRequest(copyResource, Method.POST);
            request.AddJsonBody(command);

            var response = restClient.Execute<CopyAccountResult>(request);

            return response?.Data;
        }

        public async Task<CopyAccountResult> CopyAsync(CopyAccountParams command)
        {
            var request = new RestRequest(copyResource, Method.POST);
            request.AddJsonBody(command);

            var response = await restClient.ExecuteTaskAsync<CopyAccountResult>(request);

            return response?.Data;
        }
    }
}