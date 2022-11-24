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
            ResponseDto response = await _productService.GetAllAsync(await GetToken());
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
                ResponseDto response = await _productService.CreateAsync(product, await GetToken());

                if (response?.IsSuccess ?? false)
                    return RedirectToAction(nameof(ProductIndex));

            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int id)
        {
            ResponseDto response = await _productService.GetByIdAsync(id, await GetToken());

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
                ResponseDto response = await _productService.UpdateAsync(product, await GetToken());

                if (response?.IsSuccess ?? false)
                    return RedirectToAction(nameof(ProductIndex));

            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            ResponseDto response = await _productService.GetByIdAsync(id, await GetToken());

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
                ResponseDto response = await _productService.DeleteAsync(product.ProductId, await GetToken());

                if (response?.IsSuccess ?? false)
                    return RedirectToAction(nameof(ProductIndex));
            }
            return View(product);
        }

        private async Task<string> GetToken() => await HttpContext.GetTokenAsync("access_token");
    }
}
