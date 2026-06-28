namespace Tenency.Models.Dto
{
    public class PropertySummaryDto
    {
        public Guid Guid { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal BaseRent { get; set; }
        public int TenantCount { get; set; }      // active tenants only
    }
}
