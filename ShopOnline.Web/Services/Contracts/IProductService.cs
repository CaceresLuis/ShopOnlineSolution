using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductDto> GetItem(int id);
        Task<IEnumerable<ProductDto>> GetItems();
    }
}
