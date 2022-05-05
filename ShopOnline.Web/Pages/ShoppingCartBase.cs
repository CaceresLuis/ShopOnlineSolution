using ShopOnline.Models.Dtos;
using Microsoft.AspNetCore.Components;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessenger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(1);
            }
            catch (Exception ex)
            {
                ErrorMessenger = ex.Message;
            }
        }

        protected async Task DeleteItem_Click(int id)
        {
            await ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
        }

        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    CartItemQtyUpdateDto? updateItemDto = new()
                    {
                        Qty = qty,
                        CartItemId = id
                    };
                    CartItemDto? returnetUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);
                }
                else
                {
                    CartItemDto? item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RemoveCartItem(int id)
        {
            CartItemDto? cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
        }
    }
}
