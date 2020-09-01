namespace Alpha.CopyAccount.Services
{
    using Alpha.CopyAccount.Models;
    using Alpha.CopyAccount.Params;
    using RestSharp;

    public interface ICopyAccountService
    {
        CopyAccountResult Copy(CopyAccountParams command);
    }

    public class CopyAccountService : ICopyAccountService
    {
        private readonly IRestClient restClient;

        public CopyAccountService(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        public CopyAccountResult Copy(CopyAccountParams command)
        {
            var request = new RestRequest("customer/copy", Method.POST);
            request.AddJsonBody(command);

            var response = restClient.Execute<CopyAccountResult>(request);

            return response?.Data;
        }
    }
}