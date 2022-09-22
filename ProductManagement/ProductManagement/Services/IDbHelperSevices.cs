using ProductManagement.Model;

namespace ProductManagement.Services
{
    public interface IDbHelperSevices
    {
        IQueryable<Product> GetProducts(bool readOnly = true);
        void AddProducts(Product product);
        public void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IQueryable<ProductCustomPriceMap> GetProductCustomPrice(bool readOnly = true);
        void AddProductCustom(ProductCustomPriceMap productCustomPriceMap);
        void UpdateProductCustom(ProductCustomPriceMap ProductCustomPriceMap);
        void AddCustomer(Customer customer);
        IQueryable<Customer> GetCustomer(bool readOnly = true);
    }
}
