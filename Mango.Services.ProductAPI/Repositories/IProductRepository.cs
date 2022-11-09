using Mango.Services.ProductAPI.Models.Dto.Request;

namespace Mango.Services.ProductAPI.Repositories
{
    public interface IProductRepository : IRepository
    {
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto> GetById(int id);
        Task<ProductDto> CreateUpdate(ProductDto product);
        Task<bool> Delete(int id);
    }
}
