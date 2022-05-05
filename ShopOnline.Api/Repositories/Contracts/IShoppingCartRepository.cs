using ShopOnline.Models.Dtos;
using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<CartItemDto>> GetItems(int userId);
        Task<CartItemDto?> GetItem(int id);
        Task<CartItem> AddItem(CartItem cartItem);
        Task<bool> DeleteItem(int id);
        Task<CartItemDto> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
    }
}
