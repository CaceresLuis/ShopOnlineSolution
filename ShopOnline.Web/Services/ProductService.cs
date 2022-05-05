using System.Net;
using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.GetAsync("api/Product");
                if (!response.IsSuccessStatusCode)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                HttpResponseMessage? response = await _httpClient.GetAsync($"api/Product/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(ProductDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
