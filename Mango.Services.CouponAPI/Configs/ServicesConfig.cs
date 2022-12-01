using Mango.Services.CouponAPI.Repositories;

namespace Mango.Services.CouponAPI.Configs
{
    public static class ServicesConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICouponRepository, CouponRepository>();
        }
    }
}