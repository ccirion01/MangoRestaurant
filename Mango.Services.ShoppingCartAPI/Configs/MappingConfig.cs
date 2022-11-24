using AutoMapper;

namespace Mango.Services.ShoppingCartAPI.Configs
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
