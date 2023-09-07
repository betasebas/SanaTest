using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SanaTest.Contracts.Request;
using SanaTest.Contracts.Response;
using SanaTest.Domain.Models;
using SanaTest.Domain.Util;
using SanaTest.Persistence.Caching;
using SanaTest.Service.Products;

namespace SanaTest.Service.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IProductService _productService;
        private readonly ICacheData _cacheData;
        private readonly ILogger<ShoppingCartService> _logger;

        public ShoppingCartService(IProductService productService, ICacheData cacheData, ILogger<ShoppingCartService> logger)
        {
            _productService = productService;
            _cacheData = cacheData;
            _logger = logger;
        }
        public async Task<ShoppingCartResponse> AddProductShoppingCartAsync(ShoppingCartRequest shoppingCartRequest)
        {
            ProductResponse productResponse = await _productService.GetProductsServiceAsync();
            var product = productResponse.Data.FirstOrDefault(x => x.Code.Equals(shoppingCartRequest.Code) && x.Id == shoppingCartRequest.Id);
            ValidationData(product, shoppingCartRequest.Quantity);
            string key = $"{shoppingCartRequest.CustomerId}";
            List<ProductShopping> productShopping =  _cacheData.GetDataProductsShoppingCachingasync(key);
            if(productShopping == null)
            {
                productShopping = new List<ProductShopping>();
            }
            productShopping = AddOrUpdateProducts(productShopping, product, shoppingCartRequest.Quantity);
           

            _cacheData.SetDataProductsCachingasync(key, productShopping);
            return new ShoppingCartResponse{ Code = 200, Message = "Added product", Amount = productShopping.Count};
        }

        public GenericResponse DeleteProductShoppingCart(ShoppingCartDeleteRequest shoppingCartDeleteRequest)
        {
            string key = $"{shoppingCartDeleteRequest.CustomerId}";
            List<ProductShopping> productShoppings =  _cacheData.GetDataProductsShoppingCachingasync(key);
            if(productShoppings == null || productShoppings.Count == 0)
            { 
                _logger.LogError("Empty shopping cart");
                throw new Exception("Empty shopping cart");
            }

            ProductShopping productShopping = productShoppings.FirstOrDefault(x => x.Code.Equals(shoppingCartDeleteRequest.Code) && x.Id == shoppingCartDeleteRequest.Id);
            if(productShopping == null)
            {
                _logger.LogError("The product does not exist in shopping cart");
                throw new Exception("The product does not exist in shopping cart");
            }

            productShoppings.Remove(productShopping);
            _cacheData.SetDataProductsCachingasync(key, productShoppings);
            return new GenericResponse{ Code = 200, Message = "Product removed"};
        }

        public ShoppingCartDataResponse GetAllProductShoppingCartAsync(Guid CustomerId)
        {
            string key = $"{CustomerId}";
            List<ProductShopping> productShopping =  _cacheData.GetDataProductsShoppingCachingasync(key);
            if(productShopping == null || productShopping.Count == 0)
            { 
                _logger.LogError("Empty shopping cart");
                throw new Exception("Empty shopping cart");
            }

            return new ShoppingCartDataResponse { Code = 200, Message = "ok", Data = productShopping};
        }

        private List<ProductShopping> AddOrUpdateProducts(List<ProductShopping> productShoppings, Product? product, int quantity)
        {
            ProductShopping productShopping = productShoppings.FirstOrDefault(x => x.Code.Equals(product.Code) && x.Id == product.Id);
            
            if(productShopping != null)
            {
                int index = productShoppings.IndexOf(productShopping);
                productShoppings[index].Quantity = quantity;
            }
            else
            {
                productShoppings.Add(new ProductShopping{
                    Id = product.Id,
                    Code = product.Code,
                    Description = product.Description,
                    Stock = product.Stock,
                    Image = product.Image,
                    Value = product.Value,
                    Quantity = quantity,
                });
            }

            return productShoppings; 
        }

        private void ValidationData(Product? product, int quantity)
        {
            if (product == null)
            {
                _logger.LogError("The product does not exist");
                throw new Exception("The product does not exist");
            }

            if(product.Stock < quantity)
            {
                _logger.LogError("Product quantity exceeds stock");
                throw new Exception("Product quantity exceeds stock");
            }
        }
    }
}