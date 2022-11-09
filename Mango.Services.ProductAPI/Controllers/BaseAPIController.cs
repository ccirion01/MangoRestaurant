using Mango.Services.ProductAPI.Models.Dto.Response;
using Mango.Services.ProductAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    public class BaseAPIController<TResponse, Repository> : ControllerBase
        where Repository : IRepository
    {
        protected ResponseDto<TResponse> _response;
        protected Repository _repository;

        public BaseAPIController(Repository repository)
        {
            _repository = repository;
            _response = new ResponseDto<TResponse>();
        }

        public async Task<ResponseDto<TResponse>> Execute(Func<Task<TResponse>> command)
        {
            try
            {
                TResponse response = await command();
                _response.Result = response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.AddErrorMessage(ex.ToString());
            }
            return _response;
        }
    }
}
