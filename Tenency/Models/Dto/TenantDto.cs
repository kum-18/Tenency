namespace Tenency.Models.Dto
{
    public class TenantSummaryDto
    {
        public Guid Id { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string RentType { get; set; } = string.Empty;
        public string? ShopName { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public DateOnly MoveInDate { get; set; }
        public DateOnly RentStartDate { get; set; }
        public DateOnly? EndDate { get; set; }
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
        public DateOnly MoveInDate { get; set; }
        public DateOnly RentStartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTenantPayload
    {
        public string PropertyId { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string ProofNumber { get; set; } = string.Empty;
        public string RentType { get; set; } = string.Empty;
        public decimal StartingCurrent { get; set; }
        public string? ShopName { get; set; }
    }

    public class UpdateTenantPayload
    {
        public string? TenantName { get; set; }
        public string? ShopName { get; set; }
        public string? ProofNumber { get; set; }
        public string? RentStartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
