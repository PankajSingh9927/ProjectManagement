

using Domain.Model;
using Infrastructure;
using Infrastructure.Repository;

namespace Applications.Services
{
    public class DbHelperSevices : IDbHelperSevices
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCustomPriceMap> _productCustomPriceRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly MyDbContext _context;

        public DbHelperSevices(IRepository<Product> productRepository, MyDbContext context, IRepository<ProductCustomPriceMap> productCustomPriceRepository, IRepository<Customer> customerRepository)
        {
            this._productRepository = productRepository;
            this._context = context;
            this._productCustomPriceRepository = productCustomPriceRepository;
            this._customerRepository = customerRepository;
        }

        public IQueryable<Product> GetProducts(bool readOnly = true)
        {
            IQueryable<Product> products = null;
            if (readOnly)
            {
                products = _productRepository.Init(_context).TableNoTracking;
            }
            else
            {
                products = _productRepository.Init(_context).Table;
            }
            return products;
        }

        public void AddProducts(Product product)
        {
            if(product is not null)
            _productRepository.Init(_context).Insert(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product is not null && product.Id>0)
                _productRepository.Init(_context).Update(product);
        }

        public void DeleteProduct(Product product)
        {
            if (product is not null)
                _productRepository.Init(_context).Delete(product);
        }

        public IQueryable<ProductCustomPriceMap> GetProductCustomPrice(bool readOnly = true)
        {
            IQueryable<ProductCustomPriceMap> productCustomPrice = null;
            if (readOnly)
            {
                productCustomPrice = _productCustomPriceRepository.Init(_context).TableNoTracking;
            }
            else
            {
                productCustomPrice = _productCustomPriceRepository.Init(_context).Table;
            }
            return productCustomPrice;
        }

        public void AddProductCustom(ProductCustomPriceMap productCustomPriceMap)
        {
            if (productCustomPriceMap is not null)
                _productCustomPriceRepository.Init(_context).Insert(productCustomPriceMap);
        }
        public void UpdateProductCustom(ProductCustomPriceMap ProductCustomPriceMap)
        {
            if (ProductCustomPriceMap is not null && ProductCustomPriceMap.Id > 0)
                _productCustomPriceRepository.Init(_context).Update(ProductCustomPriceMap);
        }

        #region customer crud
        public IQueryable<Customer> GetCustomer(bool readOnly = true)
        {
            IQueryable<Customer> customers = null;
            if (readOnly)
            {
                customers = _customerRepository.Init(_context).TableNoTracking;
            }
            else
            {
                customers = _customerRepository.Init(_context).Table;
            }
            return customers;
        }

        public void AddCustomer(Customer customer)
        {
            if (customer is not null)
                _customerRepository.Init(_context).Insert(customer);
        }
        #endregion
    }
}
