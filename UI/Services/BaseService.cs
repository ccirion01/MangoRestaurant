using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UI.Models;
using UI.Services.IServices;

namespace UI.Services
{
    public abstract class BaseService : IBaseService
    {
        internal abstract string Url { get; }
        public ResponseDto ResponseModel { get; set; }
        public IHttpClientFactory ClientFactory { get; set; }

        public BaseService(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
            ResponseModel = new ResponseDto();
        }

        public async Task<ResponseDto> SendAsync(ApiRequest apiRequest)
        {
            try
            {
                HttpClient client = ClientFactory.CreateClient("MangoAPI");
                //client.Timeout = TimeSpan.FromDays(1);
                HttpRequestMessage message = new HttpRequestMessage();

                message.Method = apiRequest.ApiType.ToHttpMethod();
                message.RequestUri = new Uri(apiRequest.Url);
                message.Headers.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Clear();

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

                if (apiRequest.Data != null)
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage apiResponse = await client.SendAsync(message);
                string apiContent = await apiResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            }
            catch (Exception e)
            {
                return ResponseDto.CreateWithError(e.Message);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        internal ApiRequest CreateApiRequest(
           SD.ApiType apiType,
           string token,
           string url = null,
           object data = null)
        {
            return new()
            {
                ApiType = apiType,
                Url = url ?? Url,
                Data = data,
                AccessToken = token
            };
        }
    }
}
