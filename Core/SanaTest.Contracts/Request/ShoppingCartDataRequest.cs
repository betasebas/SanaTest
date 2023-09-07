using System.ComponentModel.DataAnnotations;

namespace SanaTest.Contracts.Request
{
    public class ShoppingCartDataRequest
    {
        [Required]
        public Guid CustomerId { get; set; }
    }
}