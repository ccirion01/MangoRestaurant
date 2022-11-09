using UI.Models;

namespace UI.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        ResponseDto ResponseModel { get; set; }
        Task<ResponseDto> SendAsync(ApiRequest apiRequest);
    }
}
