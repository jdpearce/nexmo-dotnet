using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace Nexmo.Api
{
    public class Client
    {
        public string ApiKey => ConfigurationManager.AppSettings["Nexmo.api_key"];
        public string ApiSecret => ConfigurationManager.AppSettings["Nexmo.api_secret"];
        public Uri RestUrl => new Uri(ConfigurationManager.AppSettings["Nexmo.Url.Rest"]);
        public Uri ApiUril => new Uri(ConfigurationManager.AppSettings["Nexmo.Url.Api"]);

        public Client()
        {
            HttpClient = new HttpClient() {BaseAddress = RestUrl};
        }

        public Client(HttpMessageHandler handler)
        {
            HttpClient = new HttpClient(handler) { BaseAddress = RestUrl };
        }

        public HttpClient HttpClient { get; private set; }

        public T GetResource<T>(string resourceUri, Dictionary<string, string> parameters = null)
        {
            var querybuilder = HttpUtility.ParseQueryString(string.Empty);
            querybuilder.Add("api_key", ApiKey);
            querybuilder.Add("api_secret", ApiSecret);
            if (parameters != null)
                foreach (var key in parameters.Keys)
                    querybuilder.Add(key, parameters[key]);

            var responseTask = HttpClient.GetAsync($@"{resourceUri}?{querybuilder.ToString()}");
            responseTask.Wait();

            if (!responseTask.Result.IsSuccessStatusCode)
                throw new Exception("Error while retrieving resource.");

            var contentTask = responseTask.Result.Content.ReadAsAsync<T>();
            contentTask.Wait();
            return contentTask.Result;
        }
    }
}
