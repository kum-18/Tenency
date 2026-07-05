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
        public DateOnly MoveInDate { get; set; }
        public DateOnly RentStartDate { get; set; }
        public Boolean IsActive { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt
        {
            get; set;
        }
        public decimal StartingCurrent { get; set; }
        public PropertyDetails Property { get; set; } = null;
        public ICollection<MonthlyDetails> MonthlyDetails { get; set; } = new List<MonthlyDetails>();
    }
}
