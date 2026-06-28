using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tenency.Models
{
    public class Expense
    {
        public Guid Guid { get; set; }
        public Guid PropertyId { get; set; }
        public decimal AmountSpent { get; set; }
        public DateOnly ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public PropertyDetails Property { get; set; }
    }
}
