using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetByCode(string couponCode)
        {
            Coupon coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);

            if (coupon == null)
                throw new Exception("Invalid coupon code");

            return _mapper.Map<CouponDto>(coupon);
        }
    }
}