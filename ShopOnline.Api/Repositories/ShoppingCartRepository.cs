using ShopOnline.Api.Data;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Entities;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<CartItem> AddItem(CartItem cartItem)
        {
            EntityEntry<CartItem>? result = await _shopOnlineDbContext.CartItems.AddAsync(cartItem);
            await _shopOnlineDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<CartItemDto?> GetItem(int id)
        {
            return await (from cart in _shopOnlineDbContext.Carts
                          join cartItem in _shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          join product in _shopOnlineDbContext.Products
                          on cartItem.ProductId equals product.Id
                          select new CartItemDto
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              ProductName = product.Name,
                              ProductDescription = product.Description,
                              ProductImagURL = product.ImageURL,
                              Price = product.Price,
                              CartId = cartItem.CartId,
                              Qty = cartItem.Qty,
                              TotalPrice = product.Price * cartItem.Qty
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItemDto>> GetItems(int userId)
        {
            return await (from cart in _shopOnlineDbContext.Carts
                          where cart.UserId == userId
                          join cartItem in _shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          join product in _shopOnlineDbContext.Products
                          on cartItem.ProductId equals product.Id
                          select new CartItemDto
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              ProductName = product.Name,
                              ProductDescription = product.Description,
                              ProductImagURL = product.ImageURL,
                              Price = product.Price,
                              CartId = cartItem.CartId,
                              Qty = cartItem.Qty,
                              TotalPrice = product.Price * cartItem.Qty
                          }).ToListAsync();
        }

        public async Task<CartItemDto> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var itemCart = await ItemExist(id);
            if (itemCart != null)
            {
                itemCart.Qty = cartItemQtyUpdateDto.Qty;
                await _shopOnlineDbContext.SaveChangesAsync();
                return await GetItem(id);
            }
            return null;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var cartItem = await ItemExist(id);
            if (cartItem == null)
                return false;
            
            _shopOnlineDbContext.CartItems.Remove(cartItem);
            return await _shopOnlineDbContext.SaveChangesAsync() > 0;

        }

        private async Task<CartItem> ItemExist(int id) => await _shopOnlineDbContext.CartItems.FindAsync(id);
    }
}
