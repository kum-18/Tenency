namespace Tenency.Models.Dto
{
    public class MonthlyDetailsDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string PropertyName { get; set; }
        public DateOnly BillingMonth { get; set; }
        public decimal CurrentUsed { get; set; }

        public decimal CurrentReadingFrom { get; set; }
        public decimal CurrentReadingTo { get; set; }
        public decimal TotalMonthlyRent { get; set; }
        public decimal ElectricityCharges { get; set; }
        public decimal BaseRent { get; set; }
        public decimal BaseCurrentCharges { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Due { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
