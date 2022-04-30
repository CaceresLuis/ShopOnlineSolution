using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IProducRepository
    {
        Task<Product?> GetItem(int id);
        Task<IEnumerable<Product>> GetItems();
        Task<ProductCategory?> GetCategory(int id);
        Task<IEnumerable<ProductCategory>> GetCategories();
    }
}
