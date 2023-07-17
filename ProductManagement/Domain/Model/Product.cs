
namespace Domain.Model
{
    public class Product: BaseEntity
    {
        public Guid Identifier { get; set; }
        public string ProductCode { get; set; }
        public Decimal Price { get; set; }
        public DateTime CratedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
