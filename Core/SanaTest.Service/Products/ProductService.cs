using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SanaTest.Contracts.Response;
using SanaTest.Domain.Models;
using SanaTest.Persistence.Caching;
using SanaTest.Persistence.Providers.ProductProvider;

namespace SanaTest.Service.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductProvider _productProvider;
        private readonly ICacheData _cacheData;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductProvider productProvider, ICacheData cacheData, ILogger<ProductService> logger)
        {
            _productProvider = productProvider;
            _cacheData = cacheData;
            _logger = logger;
        }
        public async Task<ProductResponse> GetProductsServiceAsync()
        {
            _logger.LogInformation("Start product service");
            string keyCache = $"{DateTime.Now.ToString("yyyy-MM-dd")}_Products";
            List<Product> products = _cacheData.GetDataProductsCachingasync(keyCache);
            if(products == null || products.Count == 0)
            {
                products = await _productProvider.GetAllProductsAsync();
                ValidateProducts(products);
                _cacheData.SetDataProductsCachingasync(keyCache, products);
            }

            return new ProductResponse { Code = 200, Message = "OK", Data = products};
        }

        private void ValidateProducts(List<Product> products)
        {
            if(products == null || products.Count <= 0)
            {
                _logger.LogError("No registered products found");
                throw new Exception("No registered products found");
            }
        }
    }
}