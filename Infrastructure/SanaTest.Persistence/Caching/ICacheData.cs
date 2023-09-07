using SanaTest.Domain.Models;
using SanaTest.Domain.Util;

namespace SanaTest.Persistence.Caching
{
    public interface ICacheData
    {
        List<Product> GetDataProductsCachingasync(string key);

        void SetDataProductsCachingasync(string key, List<Product> products);

         List<ProductShopping> GetDataProductsShoppingCachingasync(string key);

        void SetDataProductsCachingasync(string key, List<ProductShopping> products);

    }
}