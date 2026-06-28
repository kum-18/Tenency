namespace Tenency.Models.Dto
{
    public class ExpenseSummaryDto
    {
        public Guid Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public decimal AmountSpent { get; set; }
        public DateOnly ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class CreateExpensePayload
    {
        public string PropertyId { get; set; } = string.Empty;
        public decimal AmountSpent { get; set; }
        public DateOnly ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateExpensePayload
    {
        public decimal? AmountSpent { get; set; }
        public DateOnly? ExpenseDate { get; set; }
        public string? Description { get; set; }
    }
}
