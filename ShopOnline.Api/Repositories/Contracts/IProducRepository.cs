using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IProducRepository
    {
        Task<ProductDto?> GetItem(int id);
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductCategory?> GetCategory(int id);
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<bool> ProductExist(int idProduct);
    }
}
