namespace SanaTest.Domain.Models
{
    public class ProductCategory : BaseModel
    {
        public Guid IdProduct { get; set; }

        public Guid IdCategory { get; set; }
    }
}