using SanaTest.Contracts.Request;
using SanaTest.Contracts.Response;

namespace SanaTest.Service.Orders
{
    public interface IOrderService
    {
        Task<GenericResponse> AddOrderAsync(OrderRequest orderRequest);
    }
}