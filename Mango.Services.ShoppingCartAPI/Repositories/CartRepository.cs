using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto.Request;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> ClearCart(string userId)
        {
            CartHeader headerFromDb = await _db.CartHeaders.FirstOrDefaultAsync(h => h.UserId == userId);

            if (headerFromDb == null)
                return false;

            _db.CartDetails.RemoveRange(_db.CartDetails.Where(d => d.CartHeaderId == headerFromDb.CartHeaderId));
            _db.CartHeaders.Remove(headerFromDb);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<CartDto> CreateUpdate(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);

            //We only add or update one detail at a time.
            CartDetail detail = cart.CartDetails.FirstOrDefault();
            Product productInDb = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == detail.ProductId);

            if (productInDb == null)
            {
                _db.Products.Add(detail.Product);
                await _db.SaveChangesAsync();
            }

            CartHeader headerInDb = await _db.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(h => h.UserId == cart.CartHeader.UserId);

            //Create
            if (headerInDb == null)
            {
                //Add the header
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                //Add the detail
                detail.CartHeaderId = cart.CartHeader.CartHeaderId;
                detail.Product = null; //Clear the product as we have already created it.
                _db.CartDetails.Add(detail);
                await _db.SaveChangesAsync();
            }
            //Update
            else
            {
                CartDetail detailInDb = await _db.CartDetails
                    .Include(d => d.Product)
                    .FirstOrDefaultAsync(d => d.CartHeaderId == headerInDb.CartHeaderId && d.ProductId == detail.ProductId);

                if (detailInDb == null)
                {
                    detail.CartHeaderId = headerInDb.CartHeaderId;
                    detail.Product = null;
                    _db.CartDetails.Add(detail);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    detailInDb.Count = detail.Count;
                    _db.CartDetails.Update(detailInDb);
                    await _db.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _db.CartHeaders.FirstOrDefaultAsync(h => h.UserId == userId)
            };
            cart.CartDetails = _db.CartDetails
                .Where(d => d.CartHeaderId == cart.CartHeader.CartHeaderId)
                .Include(d => d.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> ApplyCoupon(CartHeaderDto headerDto)
        {
            CartHeader header = await _db.CartHeaders.FirstOrDefaultAsync(h => h.UserId == headerDto.UserId);
            header.CouponCode = headerDto.CouponCode;
            _db.CartHeaders.Update(header);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveCoupon(string userId)
        {
            CartHeader header = await _db.CartHeaders.FirstOrDefaultAsync(h => h.UserId == userId);
            header.CouponCode = string.Empty;
            _db.CartHeaders.Update(header);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveDetail(int cartDetailId)
        {
            try
            {
                CartDetail detail = await _db.CartDetails.FirstOrDefaultAsync(d => d.CartDetailsId == cartDetailId);

                if (detail == null)
                    throw new Exception("Detail does not exist");

                int detailsCount = await _db.CartDetails
                    .Where(d => d.CartHeaderId == detail.CartHeaderId)
                    .CountAsync();

                _db.CartDetails.Remove(detail);

                if (detailsCount == 1)
                    _db.CartHeaders
                        .Remove(await _db.CartHeaders.FirstOrDefaultAsync(h => h.CartHeaderId == detail.CartHeaderId));

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                //TODO: Log error
                throw;
            }
        }
    }
}