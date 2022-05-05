using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;
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

        public async Task<bool> ProductExist(int idProduct)
        {
            return await _shopOnlineDbContext.Products.AnyAsync(p => p.Id == idProduct);
        }

        public async Task<ProductDto?> GetItem(int id)
        {
            return await (from productCategory in _shopOnlineDbContext.ProductCategories
                          join product in _shopOnlineDbContext.Products
                          on productCategory.Id equals product.CategoryId
                          where product.Id == id
                          select new ProductDto
                          {
                              Id = product.Id,
                              Name = product.Name,
                              Description = product.Description,
                              ImageURL = product.ImageURL,
                              Price = product.Price,
                              Qty = product.Qty,
                              CategoryId = productCategory.Id,
                              CategoryName = productCategory.Name
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            return await (from productCategory in _shopOnlineDbContext.ProductCategories
                          join product in _shopOnlineDbContext.Products
                          on productCategory.Id equals product.CategoryId
                          select new ProductDto
                          {
                              Id = product.Id,
                              Name = product.Name,
                              Description = product.Description,
                              ImageURL = product.ImageURL,
                              Price = product.Price,
                              Qty = product.Qty,
                              CategoryId = productCategory.Id,
                              CategoryName = productCategory.Name
                          }).ToListAsync();
        }

        public async Task<ProductCategory?> GetCategory(int id) => await _shopOnlineDbContext.ProductCategories.FindAsync(id);

        public async Task<IEnumerable<ProductCategory>> GetCategories() => await _shopOnlineDbContext.ProductCategories.ToListAsync();
    }
}
