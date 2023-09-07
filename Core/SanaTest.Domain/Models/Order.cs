namespace SanaTest.Domain.Models
{
    public class Order: BaseModel
    {
        public DateTime Date { get; set; }

        public decimal Value { get; set; }

        public Guid IdCustomer { get; set; }

    }
}