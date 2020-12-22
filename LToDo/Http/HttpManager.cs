using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LToDo.Http
{
    public class HttpManager
    {
        private readonly static Lazy<HttpManager> manager = new Lazy<HttpManager>(new HttpManager());

        public static HttpManager Instance
        {
            get
            {
                return manager.Value;
            }
        }


        public async Task<T> PostAsync<T>(HttpRequest request) where T : HttpResponse, new()
        {
            var httpClient = new HttpClient();
            var json = new StringContent(JsonConvert.SerializeObject(request));
            json.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccountManager.Instance.AccountToken);
            try
            {
                var result = await httpClient.PostAsync(request.Url, json);
                if (result.IsSuccessStatusCode)
                {
                    var responseString = await result.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<T>(responseString);
                    response.HttpStatusCode = (int)result.StatusCode;
                    response.IsSuccessResponse = true;
                    return response;
                }
                return new T()
                {
                    HttpStatusCode = (int)result.StatusCode,
                    IsSuccessResponse = false
                };
            }
            catch (Exception)
            {
                return new T()
                {
                    HttpStatusCode = 400,
                    IsSuccessResponse = false
                };
            }
        }
    }
}
