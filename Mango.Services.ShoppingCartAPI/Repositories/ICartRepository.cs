using Mango.Services.ShoppingCartAPI.Models.Dto.Request;

namespace Mango.Services.ShoppingCartAPI.Repositories
{
    public interface ICartRepository
    {
        Task<CartDto> GetByUserId(string userId);
        Task<CartDto> CreateUpdate(CartDto cart);
        Task<bool> RemoveDetail(int cartDetailId);
        Task<bool> ApplyCoupon(CartHeaderDto headerDto);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
