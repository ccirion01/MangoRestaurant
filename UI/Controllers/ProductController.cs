using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Services.IServices;

namespace UI.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            ResponseDto response = await _productService.GetAllAsync(await GetTokenAsync());
            List<ProductDto> products = new();

            if (IsSuccess(response))
                products = Deserialize<List<ProductDto>>(response);

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
                ResponseDto response = await _productService.CreateAsync(product, await GetTokenAsync());

                if (IsSuccess(response))
                    return RedirectToAction(nameof(ProductIndex));

            }
            return View(product);
        }

        public async Task<IActionResult> ProductEdit(int id)
        {
            ResponseDto response = await _productService.GetByIdAsync(id, await GetTokenAsync());

            if (IsSuccess(response))
            {
                ProductDto product = Deserialize<ProductDto>(response);
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
                ResponseDto response = await _productService.UpdateAsync(product, await GetTokenAsync());

                if (IsSuccess(response))
                    return RedirectToAction(nameof(ProductIndex));

            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            ResponseDto response = await _productService.GetByIdAsync(id, await GetTokenAsync());

            if (IsSuccess(response))
            {
                ProductDto product = Deserialize<ProductDto>(response);
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
                ResponseDto response = await _productService.DeleteAsync(product.ProductId, await GetTokenAsync());

                if (IsSuccess(response))
                    return RedirectToAction(nameof(ProductIndex));
            }
            return View(product);
        }
    }
}
