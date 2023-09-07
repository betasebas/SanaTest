using SanaTest.Domain.Models;

namespace SanaTest.Persistence.Providers.ProductProvider
{
    public interface IProductProvider
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}