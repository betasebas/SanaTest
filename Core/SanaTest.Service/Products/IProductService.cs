using SanaTest.Contracts.Response;

namespace SanaTest.Service.Products
{
    public interface IProductService
    {
        Task<ProductResponse> GetProductsServiceAsync();
    }
}