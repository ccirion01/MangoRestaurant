using Mango.Services.ProductAPI.Repositories;

namespace Mango.Services.ProductAPI.Configs
{
    public static class ServicesConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
