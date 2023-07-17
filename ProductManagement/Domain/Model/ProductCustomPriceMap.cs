
namespace Domain.Model
{
    public class ProductCustomPriceMap: BaseEntity
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public Decimal Price { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
