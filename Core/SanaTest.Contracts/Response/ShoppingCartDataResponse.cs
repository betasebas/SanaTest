using SanaTest.Domain.Util;

namespace SanaTest.Contracts.Response
{
    public class ShoppingCartDataResponse : GenericResponse
    {
         public List<ProductShopping> Data { get; set; }
    }
}