

namespace Tenency.Models
{
    public class MonthlyDetails
    {
        public Guid Guid { get; set; }
        public Guid TenantId { get; set; }
        public Guid PropertyId { get; set; }
        public DateOnly BillingMonth { get; set; }
        public decimal CurrentUsed { get; set; }

        public decimal CurrentReadingFrom { get; set; }
        public decimal CurrentReadingTo { get; set; }
        public decimal TotalMonthlyRent { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Due { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public  TenentDetails Tenant { get; set; }
        public  PropertyDetails Property { get; set; }
    }
}
