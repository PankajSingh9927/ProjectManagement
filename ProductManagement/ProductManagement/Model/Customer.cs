using ProductManagement.Data;

namespace ProductManagement.Model
{
    public class Customer: BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CratedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid Identifier { get; set; }

        public string Password { get; set; }
    }
}
