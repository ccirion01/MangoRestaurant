using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Models;

namespace UI.Controllers
{
    public class BaseController : Controller
    {
        public async Task<string> GetTokenAsync() => await HttpContext.GetTokenAsync("access_token");
        public string GetUserId() => User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
        public T Deserialize<T>(ResponseDto response) => JsonConvert.DeserializeObject<T>(Convert.ToString(response.Result));
        public bool IsSuccess(ResponseDto response) => response?.IsSuccess ?? false;
    }
}
