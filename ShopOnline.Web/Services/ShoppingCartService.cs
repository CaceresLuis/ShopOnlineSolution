using System.Net;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("api/ShoppingCart", cartItemToAddDto);
                if (!response.IsSuccessStatusCode)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} - {message}");
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                    return default(CartItemDto);

                return await response.Content.ReadFromJsonAsync<CartItemDto>();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (!response.IsSuccessStatusCode)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} - {message}");
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<CartItemDto>().ToList();

                return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var jsonrequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
                var content = new StringContent(jsonrequest, Encoding.UTF8, "application/json-patch+json");
                
                var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteItem(int id)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} - {message}");
                }

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
