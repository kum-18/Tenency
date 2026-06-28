using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenency.Models;

namespace Tenency.Data.Configurations
{
    public class TenantDbConfiguration: IEntityTypeConfiguration<TenentDetails>
    {
        public void Configure(EntityTypeBuilder<TenentDetails> builder)
        {
            builder.ToTable("tenant");
            builder.HasKey(t => t.Guid);
            builder.Property(t => t.Guid).HasColumnName("guid").HasDefaultValue("gen_random_uuid()");
            builder.Property(t => t.PropertyId).HasColumnName("property_id");
            builder.Property(t => t.TenantName).HasColumnName("tenant_name").HasMaxLength(200).IsRequired();
            builder.Property(t => t.ProofNumber).HasColumnName("proof_number").HasMaxLength(100).IsRequired();
            builder.Property(t => t.RentType).HasColumnName("rent_type").HasMaxLength(50).IsRequired();
            builder.Property(t => t.ShopName).HasColumnName("shop_name").HasMaxLength(300);

            builder.Property(t => t.StartDate).HasColumnName("start_date").HasColumnType("timestamp").IsRequired();
            builder.Property(t => t.EndDate).HasColumnName("end_date").HasColumnType("timestamp");
            builder.Property(m => m.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
            builder.Property(m => m.UpdatedAt).HasColumnName("updatede_at").HasColumnType("timestamp");

            builder.HasOne(t => t.Property)
                .WithMany(p => p.Tenants)
                .HasForeignKey(t => t.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
