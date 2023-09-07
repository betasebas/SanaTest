namespace SanaTest.Domain.Models
{
    public class OrderProducts : BaseModel
    {
        public Guid IdOrder { get; set; }
        public Guid IdProduct { get; set; }
        public decimal? Subvalue { get; set; }
        public int Quantity { get; set; }
    }

}