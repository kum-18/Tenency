using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenency.Models;

namespace Tenency.Data.Configurations
{
    public class ProperTabelConfiguration: IEntityTypeConfiguration<PropertyDetails>
    {
        public void Configure(EntityTypeBuilder<PropertyDetails> builder)
        {
            builder.ToTable("property");
            builder.HasKey(t => t.Guid);
            builder.Property(p => p.Guid)
                .HasColumnName("guid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(p => p.PropertyName).HasColumnName("property_name").HasMaxLength(150).IsRequired();
            builder.Property(p => p.Address).HasColumnName("address").HasMaxLength(500).IsRequired();

            builder.Property(p => p.LengthValue).HasColumnName("length_value").HasColumnType("decimal(10, 4)");
            builder.Property(p => p.WidthValue).HasColumnName("width_value").HasColumnType("decimal(10, 4)");
            builder.Property(p => p.Unit).HasColumnName("unit").HasMaxLength(10);

            builder.Property(p => p.BaseRent).HasColumnName("base_rent").HasColumnType("decimal(12, 2)").IsRequired();
            builder.Property(p => p.BaseCurrentPrice).HasColumnName("base_current_price").HasColumnType("decimal(10, 2)").IsRequired();
            builder.Property(m => m.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
            builder.Property(m => m.UpdatedAt).HasColumnName("updatede_at").HasColumnType("timestamp");
        }
    }
}
