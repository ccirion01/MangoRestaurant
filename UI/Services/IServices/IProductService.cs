using UI.Models;

namespace UI.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<ResponseDto> GetAllAsync(string token);
        Task<ResponseDto> GetByIdAsync(int id, string token);
        Task<ResponseDto> CreateAsync(ProductDto product, string token);
        Task<ResponseDto> UpdateAsync(ProductDto product, string token);
        Task<ResponseDto> DeleteAsync(int id, string token);
    }
}
