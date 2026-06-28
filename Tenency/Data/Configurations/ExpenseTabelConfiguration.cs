using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenency.Models;

namespace Tenency.Data.Configurations
{
    public class ExpenseTabelConfiguration: IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("expense");

            builder.HasKey(e => e.Guid);
            builder.Property(e => e.Guid).HasColumnName("guid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(e => e.PropertyId).HasColumnName("property_id");
            builder.Property(e => e.AmountSpent).HasColumnName("amount_spent").HasColumnType("numeric(10, 2)").IsRequired();
            builder.Property(e => e.ExpenseDate).HasColumnName("expense_date").HasColumnType("date").IsRequired();
            builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
            builder.Property(m => m.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
            builder.Property(m => m.UpdatedAt).HasColumnName("updatede_at").HasColumnType("timestamp");

            // Foreign Key Relationship
            builder.HasOne(e => e.Property)
                   .WithMany(p => p.Expenses)
                   .HasForeignKey(e => e.PropertyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
