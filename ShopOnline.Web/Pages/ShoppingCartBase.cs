using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using Microsoft.AspNetCore.Components;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessenger { get; set; }

        public string TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(1);
                CartChanged();
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
            CartChanged();
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
                    UpdateItemTotalPrice(returnetUpdateItemDto);

                    CartChanged();

                    await MakeUpdateQtyButtonVisible(id, false);
                }
                else
                {
                    CartItemDto? item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }

                    CartChanged();
                    await MakeUpdateQtyButtonVisible(id, false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }

        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }


        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
        }

        private void CalculateCartSumaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString();
        }
        
        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(p => p.Qty);
        }

        private void RemoveCartItem(int id)
        {
            CartItemDto? cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
            UpdateItemTotalPrice(cartItemDto);
            CalculateCartSumaryTotals();
        }

        private void CartChanged()
        {
            CalculateCartSumaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanget(TotalQuantity);
        }
    }
}
