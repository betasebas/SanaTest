using SanaTest.Domain.Models;

namespace SanaTest.Persistence.Providers.OrderProvider
{
    public interface IOrdersProvider
    {
        Task<Guid> AddOrderAsync(Order order, List<OrderProducts> orderProducts);
    }
}