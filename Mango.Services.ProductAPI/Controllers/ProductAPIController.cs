using Mango.Services.ProductAPI.Models.Dto.Request;
using Mango.Services.ProductAPI.Models.Dto.Response;
using Mango.Services.ProductAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController
    {        
        private IProductRepository _repository;

        public ProductAPIController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ResponseDto<IEnumerable<ProductDto>>> Get()
        {
            //return await Execute(() => await _repository.GetAll());
            ResponseDto<IEnumerable<ProductDto>> response = new();            
            try
            {
                response.Result = await _repository.GetAll();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddErrorMessage(ex.ToString());
            }
            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ResponseDto<ProductDto>> Get(int id)
        {
            ResponseDto<ProductDto> response = new();
            try
            {
                response.Result = await _repository.GetById(id);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddErrorMessage(ex.ToString());
            }
            return response;
        }

        [HttpPost]
        [Authorize]
        public async Task<ResponseDto<ProductDto>> Post([FromBody]ProductDto dto)
        {
            ResponseDto<ProductDto> response = new();
            try
            {
                response.Result = await _repository.CreateUpdate(dto);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddErrorMessage(ex.ToString());
            }
            return response;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseDto<ProductDto>> Put([FromBody] ProductDto dto)
        {
            ResponseDto<ProductDto> response = new();
            try
            {
                response.Result = await _repository.CreateUpdate(dto);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddErrorMessage(ex.ToString());
            }
            return response;
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = SD.Admin)]
        public async Task<ResponseDto<bool>> Delete(int id)
        {
            ResponseDto<bool> response = new();
            try
            {
                response.Result = await _repository.Delete(id);
                response.IsSuccess = response.Result;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.AddErrorMessage(ex.ToString());
            }
            return response;
        }
    }
}
