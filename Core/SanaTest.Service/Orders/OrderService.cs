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
using SanaTest.Persistence.Providers.OrderProvider;

namespace SanaTest.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICacheData _cacheData;
        private readonly IOrdersProvider _ordersProvider;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ICacheData cacheData, IOrdersProvider ordersProvider, ILogger<OrderService> logger)
        {
            _cacheData = cacheData;
            _ordersProvider = ordersProvider;
            _logger = logger;
        }
        public async Task<GenericResponse> AddOrderAsync(OrderRequest orderRequest)
        {
            string key = $"{orderRequest.IdCustomer}";
            List<ProductShopping> productShoppings =  _cacheData.GetDataProductsShoppingCachingasync(key);
            if(productShoppings == null || productShoppings.Count == 0)
            { 
                _logger.LogError("Empty shopping cart");
                throw new Exception("Empty shopping cart");
            }

            (List<OrderProducts> orderProducts, decimal? valueTotal) = GetDataOrderProduct(productShoppings);
            Order order = GetDataOrder(valueTotal, orderRequest.IdCustomer);
            _ = await _ordersProvider.AddOrderAsync(order, orderProducts);
            _cacheData.DeleteCachingShoppinCart(key);
            return new GenericResponse{ Code = 200, Message = "Created order" };
        }

        private Order GetDataOrder(decimal? valueTotal, Guid idCustomer)
        {
            return new Order
            {
                Date = DateTime.Now,
                Value = Convert.ToDecimal(valueTotal),
                IdCustomer = idCustomer
            };
        }

        private (List<OrderProducts> orderProducts, decimal? valueTotal) GetDataOrderProduct(List<ProductShopping> productShoppings)
        {
            decimal? valueTotal = 0;
            List<OrderProducts> orderProducts = new List<OrderProducts>();
            foreach(var item in productShoppings)
            {
                decimal? subValue = item.Value * item.Quantity;
                valueTotal += subValue;
                orderProducts.Add(new OrderProducts
                {
                    IdProduct = item.Id,
                    Subvalue = subValue,
                    Quantity = item.Quantity 
                });
            }

            return (orderProducts, valueTotal);
        }
    }
}