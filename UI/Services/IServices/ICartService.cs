using UI.Models;

namespace UI.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto> GetByUserIdAsync(string userId, string token);
        Task<ResponseDto> AddToCartAsync(CartDto cart, string token);
        Task<ResponseDto> UpdateCartAsync(CartDto cart, string token);
        Task<ResponseDto> RemoveFromCartAsync(int cartDetailId, string token);
    }
}
