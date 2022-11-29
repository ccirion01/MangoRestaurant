namespace Mango.Services.ShoppingCartAPI.Models.Dto.Request
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailDto> CartDetails { get; set; }
    }
}
