using System;
using System.Net.Http;
using System.Threading.Tasks;
using DictaFoule.Mobile.iOS.Business;
using Newtonsoft.Json;

namespace DictaFoule.Mobile.iOS.API
{
    public class HttpClientService
    {
        #region constante

        private const string apiDev = "https://dev-api-dictafoule.azurewebsites.net/v1/";
        //private const string api = "https://api-dictafoule.azurewebsites.net/v1/";
        private const string healtcheck = "HealthCheck/HealthChecking";

        #endregion

        private readonly static HttpClient httpClient = new HttpClient();

        public HttpClientService()
        {
            httpClient.BaseAddress = new Uri(api);
        }

        public async Task<T> PostService<T>(string url, StringContent content)
        {
            var response = await httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<T>(message);
                return item;
            }

            throw new RequestException(response.StatusCode, response.ReasonPhrase, await response.Content.ReadAsStringAsync());
        }

        public async Task<T> GetService<T>(string url)
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<T>(content);
                return item;
            }

                throw new RequestException(response.StatusCode, response.ReasonPhrase, await response.Content.ReadAsStringAsync());

        }

        public async Task<bool> HeathCheckService()
        {
            try
            {
                var response = await httpClient.GetAsync(healtcheck);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
