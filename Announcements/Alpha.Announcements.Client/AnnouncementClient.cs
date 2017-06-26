namespace Alpha.Announcements.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Configurations;
    using Helpers;
    using Models;
    using Newtonsoft.Json;
    using Requests;
    using RestSharp;

    public interface AnnouncementClient
    {
        IEnumerable<Announcement> GetAnnouncements(QueryAnnouncementRequest request);
    }

    public class AnnouncementClientImpl : AnnouncementClient
    {
        public IEnumerable<Announcement> GetAnnouncements(QueryAnnouncementRequest request)
        {
            var restClient = GetRestClient();
            var restRequest = new RestRequest("/Announcementapi/GetAnnouncements", Method.POST);
            restRequest.AddJsonBody(request);
            IRestResponse response = restClient.Execute(restRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                return JsonConvert.DeserializeObject<IEnumerable<Announcement>>(
                    response.Content,
                    new SpecificationConverter());
            }

            return Enumerable.Empty<Announcement>();
        }

        private IRestClient GetRestClient()
        {
            var restClient = new RestClient(new Uri(AppSettings.ApiUrl));

            restClient.Authenticator = new HmacAuthenticator();

            return restClient;
        }
    }
}