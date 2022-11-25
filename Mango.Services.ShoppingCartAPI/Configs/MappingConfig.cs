using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto.Request;

namespace Mango.Services.ShoppingCartAPI.Configs
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailDto>().ReverseMap();
                config.CreateMap<Cart, CartDto>().ReverseMap();
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
