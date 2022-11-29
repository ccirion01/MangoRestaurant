using UI.Models;
using UI.Services.IServices;

namespace UI.Services
{
    public class ProductService : BaseService, IProductService
    {
        internal override string Url  => SD.ProductAPIBase + "api/products";
        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        public async Task<ResponseDto> CreateAsync(ProductDto product, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.POST, token, data: product));
        }

        public async Task<ResponseDto> DeleteAsync(int id, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.DELETE, token, $"{Url}/{id}"));
        }        

        public async Task<ResponseDto> GetAllAsync(string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.GET, token));
        }

        public async Task<ResponseDto> GetByIdAsync(int id, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.GET, token, $"{Url}/{id}"));
        }

        public async Task<ResponseDto> UpdateAsync(ProductDto product, string token)
        {
            return await SendAsync(
                CreateApiRequest(SD.ApiType.PUT, token, data: product));
        }
    }   
}
