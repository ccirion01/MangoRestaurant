using Mango.Services.ShoppingCartAPI.Repositories;

namespace Mango.Services.ShoppingCartAPI.Configs
{
    public static class ServicesConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICartRepository, CartRepository>();
        }
    }
}
