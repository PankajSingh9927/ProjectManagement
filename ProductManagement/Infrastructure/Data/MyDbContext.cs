using Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure
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
            Assembly modelInAssembly = Assembly.GetExecutingAssembly();
            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
            var exportedTypes = modelInAssembly.ExportedTypes;

            foreach (Type type in exportedTypes)
            {
                if (type.BaseType == typeof(BaseEntity))
                {
                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
                }
            }

            base.OnModelCreating(modelBuilder);

            var assemblyWithMappings = Assembly.Load("Domain");
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithMappings);
        }
        //public DbSet<Product> Product { get; set; }
        //public DbSet<Customer> Customer { get; set; }
        //public DbSet<ProductCustomPriceMap> ProductCustomPriceMap { get; set; }
    }
}
