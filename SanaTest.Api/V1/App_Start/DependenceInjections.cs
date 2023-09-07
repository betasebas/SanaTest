using SanaTest.Persistence.Caching;
using SanaTest.Persistence.Providers.OrderProvider;
using SanaTest.Persistence.Providers.ProductProvider;
using SanaTest.Service.Orders;
using SanaTest.Service.Products;
using SanaTest.Service.ShoppingCart;

namespace SanaTest.Api.V1.App_Start
{
    public static class DependenceInjections
    {
        public static void AddDepedencesInjection(this IServiceCollection services)
        {
            services.AddScoped<ICacheData, CacheData>();
            services.AddScoped<IProductProvider, ProductProvider>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrdersProvider, OrdersProvider>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}