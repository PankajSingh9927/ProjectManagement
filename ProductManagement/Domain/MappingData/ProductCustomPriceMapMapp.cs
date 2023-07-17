using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.MappingData
{
    internal class ProductCustomPriceMapMapp : IEntityTypeConfiguration<ProductCustomPriceMap>
    {
        public void Configure(EntityTypeBuilder<ProductCustomPriceMap> builder)
        {
            builder.ToTable("ProductCustomPriceMap");

            builder.HasKey(ba => ba.Id);
            builder.HasOne(c => c.Customer).WithMany().HasForeignKey(c => c.CustomerId);
            builder.HasOne(c => c.Product).WithMany().HasForeignKey(c => c.ProductId);

        }
    }
}
