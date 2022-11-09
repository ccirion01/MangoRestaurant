using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using UI.Models;
using UI.Services.IServices;

namespace UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            ResponseDto response = await _productService.GetAllAsync(token);
            List<ProductDto> products = new();

            if (response?.IsSuccess ?? false)
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

            return View(products);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                ResponseDto response = await _productService.CreateAsync(product, token);

                if (response?.IsSuccess ?? false)
                    return RedirectToAction(nameof(ProductIndex));

            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            ResponseDto response = await _productService.GetByIdAsync(id, token);

            if (response?.IsSuccess ?? false)
            {
                ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                ResponseDto response = await _productService.UpdateAsync(product, token);

                if (response?.IsSuccess ?? false)
                    return RedirectToAction(nameof(ProductIndex));

            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            ResponseDto response = await _productService.GetByIdAsync(id, token);

            if (response?.IsSuccess ?? false)
            {
                ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                ResponseDto response = await _productService.DeleteAsync(product.ProductId, token);

                if (response?.IsSuccess ?? false)
                    return RedirectToAction(nameof(ProductIndex));
            }
            return View(product);
        }
    }
}
