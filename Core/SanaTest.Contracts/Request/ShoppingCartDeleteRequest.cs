namespace SanaTest.Contracts.Request
{
    public class ShoppingCartDeleteRequest
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid CustomerId { get; set; }
    }
}