using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using UI.Models;
using UI.Services.IServices;

namespace UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IProductService productService, 
            ICartService cartService, 
            ILogger<HomeController> logger)
        {
            _productService = productService;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ResponseDto response = await _productService.GetAllAsync(await GetTokenAsync());
            List<ProductDto> products = new();

            if (response?.IsSuccess ?? false)
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

            return View(products);
        }
        
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            //TODO: Fetch 'Count' value from the Cart API
            ResponseDto response = await _productService.GetByIdAsync(productId, await GetTokenAsync());
            ProductDto model = new();

            if (response?.IsSuccess ?? false)
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

            return View(model);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDto productDto)
        {
            var token = await GetTokenAsync();
            CartDto cartDto = new()
            {
                CartHeader = new()
                {
                    UserId = GetUserId()
                },
                CartDetails = new List<CartDetailDto>()
                {
                    new()
                    {
                        Count = productDto.Count,
                        ProductId = productDto.ProductId                        
                    }
                }
            };
            var product = await _productService.GetByIdAsync(productDto.ProductId, token);            
            if (product?.IsSuccess ?? false)
                cartDto.CartDetails.FirstOrDefault().Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(product.Result));

            ResponseDto response = await _cartService.AddToCartAsync(cartDto, token);

            if (response?.IsSuccess ?? false)
                return RedirectToAction(nameof(Index));

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [Authorize]
        public IActionResult Login()
        {            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
} 