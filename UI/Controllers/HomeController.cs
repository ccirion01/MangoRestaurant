using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            if (IsSuccess(response))
                products = Deserialize<List<ProductDto>>(response);

            return View(products);
        }
        
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            //TODO: Fetch 'Count' value from the Cart API
            ResponseDto response = await _productService.GetByIdAsync(productId, await GetTokenAsync());
            ProductDto model = new();

            if (IsSuccess(response))
                model = Deserialize<ProductDto>(response);

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
            if (IsSuccess(product))
                cartDto.CartDetails.FirstOrDefault().Product = Deserialize<ProductDto>(product);

            ResponseDto response = await _cartService.AddToCartAsync(cartDto, token);

            if (IsSuccess(response))
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