using UI.Models;
using UI.Services.IServices;

namespace UI.Services
{
    public class CartService : BaseService, ICartService
    {
        internal override string Url => SD.ShoppingCartAPIBase + "api/cart";
        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        public async Task<ResponseDto> GetByUserIdAsync(string userId, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.GET, token, $"{Url}/{userId}"));
        }
        
        public async Task<ResponseDto> AddToCartAsync(CartDto cart, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.POST, token, data: cart));
        }

        public async Task<ResponseDto> UpdateCartAsync(CartDto cart, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.PUT, token, data: cart));
        }

        public async Task<ResponseDto> RemoveFromCartAsync(int cartDetailId, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.DELETE, token, $"{Url}/{cartDetailId}"));
        }
    }
}
