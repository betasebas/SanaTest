namespace SanaTest.Domain.Models
{
    public class Product : BaseModel
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public int? Stock { get; set; }

        public string Image { get; set; }

        public decimal? Value { get; set; }
    }
}