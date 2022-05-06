using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        event Action<int> OnshoppingCartChanged;

        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<bool> DeleteItem(int id);
        Task<List<CartItemDto>> GetItems(int userId);
        void RaiseEventOnShoppingCartChanget(int totalQty);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);
    }
}
