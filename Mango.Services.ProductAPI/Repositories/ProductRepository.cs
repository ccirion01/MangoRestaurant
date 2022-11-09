using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto.Request;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdate(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);            
            
            if (product.ProductId > 0)
                _db.Products.Update(product);
            else
                _db.Products.Add(product);
            
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Product product = await _db.Products.FindAsync(id);
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = await _db.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
