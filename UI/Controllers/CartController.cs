using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Services.IServices;

namespace UI.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadUserCart());
        }

        public async Task<IActionResult> Remove(int cartDetailId)
        {
            await _cartService.RemoveFromCartAsync(cartDetailId, await GetTokenAsync());

            return RedirectToAction(nameof(CartIndex));
        }

        private async Task<CartDto> LoadUserCart()
        {
            ResponseDto response = await _cartService.GetByUserIdAsync(GetUserId(), await GetTokenAsync());

            CartDto cart = new();
            if (IsSuccess(response))
            {
                cart = Deserialize<CartDto>(response);

                if (cart.CartHeader != null)
                {
                    foreach (CartDetailDto detail in cart.CartDetails)
                    {
                        cart.CartHeader.OrderTotal += detail.Count * detail.Product.Price;
                    }
                }
            }
            return cart;
        }
    }
}
