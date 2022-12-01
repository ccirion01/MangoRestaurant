using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Configs
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
