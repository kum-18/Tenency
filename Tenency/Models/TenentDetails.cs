namespace Tenency.Models
{
    public class TenentDetails
    {
        public Guid Guid { get; set; }
        public Guid PropertyId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string ProofNumber { get; set; } = string.Empty;
        public string RentType { get; set; } = string.Empty;
        public string? ShopName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt
        {
            get; set;
        }
        public PropertyDetails Property { get; set; } = null;
        public ICollection<MonthlyDetails> MonthlyDetails { get; set; } = new List<MonthlyDetails>();
    }
}
