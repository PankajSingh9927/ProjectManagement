using ProductManagement.Data;

namespace ProductManagement.Model
{
    public class ProductCustomPriceMap: BaseEntity
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public Decimal Price { get; set; }
    }
}
