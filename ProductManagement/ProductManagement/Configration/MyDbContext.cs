using Microsoft.EntityFrameworkCore;
using ProductManagement.Model;

namespace ProductManagement.Configration
{
    public class MyDbContext : DbContext
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public MyDbContext(
            DbContextOptions<MyDbContext> options
        ) : base(options)
        {
            this._options = options;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<ProductCustomPriceMap> ProductCustomPriceMap { get; set; }
    }
}
