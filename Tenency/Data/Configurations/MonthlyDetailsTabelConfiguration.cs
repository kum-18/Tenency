using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenency.Models;

namespace Tenency.Data.Configurations
{
    public class MonthlyDetailsTabelConfiguration : IEntityTypeConfiguration<MonthlyDetails>
    {
        public void Configure(EntityTypeBuilder<MonthlyDetails> builder)
        {
            builder.ToTable("monthly_details");

            builder.HasKey(m => m.Guid);
            builder.Property(m => m.Guid).HasColumnName("guid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(m => m.TenantId).HasColumnName("tenant_id");
            builder.Property(m => m.PropertyId).HasColumnName("property_id");
            builder.Property(m => m.BillingMonth).HasColumnName("billing_month").HasColumnType("timestamp").IsRequired();
            builder.Property(m => m.CurrentReadingFrom).HasColumnName("current_reading_from").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(m => m.CurrentReadingTo).HasColumnName("current_reading_to").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(m => m.CurrentUsed).HasColumnName("current_used").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(m => m.TotalMonthlyRent).HasColumnName("total_montly_rent").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(m => m.AmountPaid).HasColumnName("amount_paid").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(m => m.Due).HasColumnName("due").HasColumnType("numeric(12, 2)").IsRequired();
            builder.Property(m => m.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
            builder.Property(m => m.UpdatedAt).HasColumnName("updatede_at").HasColumnType("timestamp");

            builder.HasOne(m => m.Tenant)
                .WithMany(t => t.MonthlyDetails)
                .HasForeignKey(m => m.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Property)
                .WithMany(p => p.MonthlyDetails)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
