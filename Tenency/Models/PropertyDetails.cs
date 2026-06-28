namespace Tenency.Models
{
    public class PropertyDetails
    {
        public Guid Guid { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal? LengthValue { get; set; }
        public decimal? WidthValue { get; set; }
        public string? Unit { get; set; }
        public decimal BaseRent { get; set; }
        public decimal BaseCurrentPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<TenentDetails> Tenants { get; set; } = new List<TenentDetails>();
        public ICollection<MonthlyDetails> MonthlyDetails { get; set; } = new List<MonthlyDetails>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
