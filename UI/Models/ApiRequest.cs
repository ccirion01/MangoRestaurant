using static UI.SD;

namespace UI.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }

        public static ApiRequest Create(
            ApiType apiType, 
            string url, 
            string accessToken,
            object data = null)
        {
            return new ApiRequest()
            {

            };
        }
    }
}
