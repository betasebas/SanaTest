using SanaTest.Contracts.Request;
using SanaTest.Contracts.Response;

namespace SanaTest.Service.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartResponse> AddProductShoppingCartAsync(ShoppingCartRequest shoppingCartRequest);

        ShoppingCartDataResponse GetAllProductShoppingCartAsync(Guid CustomerId);

        GenericResponse DeleteProductShoppingCart(ShoppingCartDeleteRequest shoppingCartDeleteRequest);
    }
}