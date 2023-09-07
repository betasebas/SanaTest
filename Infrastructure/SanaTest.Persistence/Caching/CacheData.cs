using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SanaTest.Domain.Models;
using SanaTest.Domain.Util;

namespace SanaTest.Persistence.Caching
{
    public class CacheData : ICacheData
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheData> _logger;
        public CacheData(ILogger<CacheData> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public List<Product> GetDataProductsCachingasync(string key)
        {     
            List<Product> products = new List<Product>();
             _logger.LogInformation("Start Get products in caching - {key}", key);
            try
            {
                _ = _cache.TryGetValue(key, out  products);
                return products;
            }catch(Exception ex)
            {
                 _logger.LogError($"Se presento un error al consultar datos en cache {ex.Message}");
                 return products;
            }          
        }

        public List<ProductShopping> GetDataProductsShoppingCachingasync(string key)
        {
              List<ProductShopping> products = new List<ProductShopping>();
             _logger.LogInformation("Start Get products shopping cart in caching - {key}", key);
            try
            {
                _ = _cache.TryGetValue(key, out products);
                return products;
            }catch(Exception ex)
            {
                 _logger.LogError($"Error in caching {ex.Message}");
                 return products;
            }          
        }

        public void SetDataProductsCachingasync(string key, List<Product> data)
        {
              _logger.LogInformation("Star set products in cachig {key}", key);
            try
            {
                _ = _cache.Set(key, data);
            }catch(Exception ex)
            {
                 _logger.LogError($"Error in caching {ex.Message}");
            }      
        }

        public void SetDataProductsCachingasync(string key, List<ProductShopping> products)
        {
              _logger.LogInformation("Star set products shopping cart in cachig {key}", key);
            try
            {
                _ = _cache.Set(key, products);
            }catch(Exception ex)
            {
                 _logger.LogError($"Error in caching {ex.Message}");
            }      
        }
    }
}