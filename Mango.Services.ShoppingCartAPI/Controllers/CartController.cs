using Mango.Services.ShoppingCartAPI.Models.Dto.Request;
using Mango.Services.ShoppingCartAPI.Models.Dto.Response;
using Mango.Services.ShoppingCartAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    public class CartController
    {        
        private ICartRepository _repository;
        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}")]
        public async Task<ResponseDto<CartDto>> Get(string userId)
        {
            return await Execute(
                async (userId) => await _repository.GetByUserId(userId), 
                userId);
        }

        [HttpPost]
        [Authorize]
        public async Task<ResponseDto<CartDto>> Create([FromBody]CartDto dto)
        {
            return await CreateUpdate(dto);
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseDto<CartDto>> Update([FromBody]CartDto dto)
        {
            return await CreateUpdate(dto);
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ResponseDto<bool>> Clear(string userId)
        {
            return await ExecuteBool(
                async (userId) => await _repository.ClearCart(userId),
                userId);
        }

        [HttpDelete]
        [Route("remove/{cartDetailId}")]
        public async Task<ResponseDto<bool>> RemoveDetail(int cartDetailId)
        {
            return await ExecuteBool(
                async (cartDetailId) => await _repository.RemoveDetail(cartDetailId),
                cartDetailId);
        }

        #region Private Methods
        private async Task<ResponseDto<CartDto>> CreateUpdate(CartDto dto)
        {
            return await Execute(
                async (dto) => await _repository.CreateUpdate(dto),
                dto);
        }

        private async Task<ResponseDto<CartDto>> Execute<T>
            (Func<T, Task<CartDto>> action, T args)
        {
            ResponseDto<CartDto> response = new();
            try
            {
                response.Result = await action(args);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddExceptionMessage(ex);
            }
            return response;
        }

        private async Task<ResponseDto<bool>> ExecuteBool<T>
            (Func<T, Task<bool>> action, T args)
        {
            ResponseDto<bool> response = new();
            try
            {
                response.Result = await action(args);
                response.IsSuccess = response.Result;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddExceptionMessage(ex);
            }
            return response;
        } 
        #endregion
    }
}
