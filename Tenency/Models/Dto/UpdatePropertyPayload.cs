namespace Tenency.Models.Dto
{
    public class UpdatePropertyPayload
    {
        public string? PropertyName { get; set; }
        public string? Address { get; set; }
        public decimal? LengthValue { get; set; }
        public decimal? WidthValue { get; set; }
        public string? Unit { get; set; }
        public decimal? BaseRent { get; set; }
        public decimal? BaseCurrentPrice { get; set; }
    }
}
