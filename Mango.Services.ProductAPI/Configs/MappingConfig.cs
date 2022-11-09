using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto.Request;

namespace Mango.Services.ProductAPI.Configs
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
