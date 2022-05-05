using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<bool> DeleteItem(int id);
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);
    }
}
