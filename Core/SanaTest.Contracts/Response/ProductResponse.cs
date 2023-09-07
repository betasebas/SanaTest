using SanaTest.Domain.Models;

namespace SanaTest.Contracts.Response
{
    public class ProductResponse : GenericResponse
    {
        public List<Product> Data { get; set; }
    }
}