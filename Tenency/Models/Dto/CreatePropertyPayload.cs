namespace Tenency.Models.Dto
{
    public class CreatePropertyPayload
    {
        public string PropertyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal? LengthValue { get; set; }
        public decimal? WidthValue { get; set; }
        public string? Unit { get; set; }
        public decimal BaseRent { get; set; }
        public decimal BaseCurrentPrice { get; set; }
    }
}
