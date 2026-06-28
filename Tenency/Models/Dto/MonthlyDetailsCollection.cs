namespace Tenency.Models.Dto
{
    public class MonthlyDetailsCollection
    {
        public Guid Guid { get; set; }
        public DateOnly BillingMonth { get; set; }
        public decimal TotalMonthlyRent { get; set; }
        public decimal AmountPaid { get; set; }
        public string PropertyName { get; set; }
    }
}
