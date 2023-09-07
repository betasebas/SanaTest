namespace SanaTest.Contracts.Request
{
    public class ShoppingCartRequest 
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public Guid CustomerId { get; set; }
    }
}