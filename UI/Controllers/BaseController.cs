using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class BaseController : Controller
    {
        public async Task<string> GetTokenAsync() => await HttpContext.GetTokenAsync("access_token");
        public string GetUserId() => User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
    }
}
