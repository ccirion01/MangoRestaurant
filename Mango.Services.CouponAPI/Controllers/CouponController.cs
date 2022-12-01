using Mango.Services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.Models.Dto.Response;
using Mango.Services.CouponAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    public class CouponController
    {
        private ICouponRepository _repository;
        public CouponController(ICouponRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        [Route("{couponCode}")]
        public async Task<ResponseDto<CouponDto>> Get(string couponCode)
        {
            ResponseDto<CouponDto> response = new();
            try
            {
                response.Result = await _repository.GetByCode(couponCode);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddExceptionMessage(ex);
            }
            return response;
        }
    }
}
