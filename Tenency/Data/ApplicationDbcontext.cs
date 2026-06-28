using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tenency.Models;

namespace Tenency.Data
{
    public class ApplicationDbcontext : DbContext

    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base() { }
        public DbSet<TenentDetails> Tenants => Set<TenentDetails>();
        public DbSet<PropertyDetails> Properties => Set<PropertyDetails>();
        public DbSet<MonthlyDetails> MonthlyDetails => Set<MonthlyDetails>();
        public DbSet<Expense> Expenses => Set<Expense>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
