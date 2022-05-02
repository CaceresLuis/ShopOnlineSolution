using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class ProducRepository : IProducRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;

        public ProducRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<Product?> GetItem(int id) => 
            await _shopOnlineDbContext.Products
            .Include(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetItems() =>
             await _shopOnlineDbContext.Products
            .Include(p => p.Category)
            .ToListAsync();

        public async Task<ProductCategory?> GetCategory(int id) => await _shopOnlineDbContext.ProductCategories.FindAsync(id);

        public async Task<IEnumerable<ProductCategory>> GetCategories() => await _shopOnlineDbContext.ProductCategories.ToListAsync();
    }
}
