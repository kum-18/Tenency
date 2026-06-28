namespace Tenency.Models.Dto
{
    public class TenantSummaryDto
    {
        public Guid Id { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string RentType { get; set; } = string.Empty;
        public string? ShopName { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class TenantDetailsDto
    {
        public Guid Id { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string ProofNumber { get; set; } = string.Empty;
        public string RentType { get; set; } = string.Empty;
        public string? ShopName { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public Guid PropertyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTenantPayload
    {
        public string PropertyId { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string ProofNumber { get; set; } = string.Empty;
        public string RentType { get; set; } = string.Empty;
        public string? ShopName { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class UpdateTenantPayload
    {
        public string? TenantName { get; set; }
        public string? ShopName { get; set; }
        public string? ProofNumber { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
