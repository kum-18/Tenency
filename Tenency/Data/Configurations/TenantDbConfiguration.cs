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
            builder.Property(t => t.StartingCurrent).HasColumnName("current_reading_from").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(t => t.MoveInDate).HasColumnName("move_in_date").HasColumnType("date").IsRequired();
            builder.Property(t => t.RentStartDate).HasColumnName("rent_start_date").HasColumnType("date").IsRequired();
            builder.Property(t => t.IsActive).HasColumnName("is_active").HasColumnType("date").IsRequired();
            builder.Property(t => t.EndDate).HasColumnName("end_date").HasColumnType("date");
            builder.Property(m => m.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
            builder.Property(m => m.UpdatedAt).HasColumnName("updatede_at").HasColumnType("timestamp");

            builder.HasOne(t => t.Property)
                .WithMany(p => p.Tenants)
                .HasForeignKey(t => t.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
