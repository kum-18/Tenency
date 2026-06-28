using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tenency.Data;
using Tenency.Models;
using Tenency.Models.Dto;

namespace Tenency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;

        public ExpenseController(ApplicationDbcontext db)
        {
            _db = db;
        }

        // GET api/expense?propertyId={propertyId}
        // GET api/expense (all expenses)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseSummaryDto>>> GetAll(
            [FromQuery] string? propertyId)
        {
            var query = _db.Expenses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(propertyId))
            {
                if (!Guid.TryParse(propertyId, out Guid validPropertyId))
                    return BadRequest("Invalid propertyId");

                query = query.Where(e => e.PropertyId == validPropertyId);
            }

            var expenses = await query
                .Select(e => new ExpenseSummaryDto
                {
                    Id = e.Guid,
                    PropertyName = e.Property.PropertyName,
                    AmountSpent = e.AmountSpent,
                    ExpenseDate = e.ExpenseDate,
                    Description = e.Description
                })
                .OrderByDescending(e => e.ExpenseDate)
                .AsNoTracking()
                .ToListAsync();

            return Ok(expenses);
        }

        // GET api/expense/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseSummaryDto>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid expense id");

            var expense = await _db.Expenses
                .Where(e => e.Guid == validId)
                .Select(e => new ExpenseSummaryDto
                {
                    Id = e.Guid,
                    PropertyName = e.Property.PropertyName,
                    AmountSpent = e.AmountSpent,
                    ExpenseDate = e.ExpenseDate,
                    Description = e.Description
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (expense == null)
                return NotFound("Expense not found");

            return Ok(expense);
        }

        // POST api/expense
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpensePayload payload)
        {
            if (string.IsNullOrWhiteSpace(payload.PropertyId) ||
                string.IsNullOrWhiteSpace(payload.Description) ||
                payload.AmountSpent <= 0)
            {
                return BadRequest("Fill all required fields correctly");
            }

            if (!Guid.TryParse(payload.PropertyId, out Guid validPropertyId))
                return BadRequest("Invalid propertyId");

            var property = await _db.Properties.FirstOrDefaultAsync(p => p.Guid == validPropertyId);
            if (property == null)
                return NotFound("Property not found");

            var expense = new Expense
            {
                PropertyId = validPropertyId,
                AmountSpent = payload.AmountSpent,
                ExpenseDate = payload.ExpenseDate,
                Description = payload.Description,
                CreatedAt = DateTime.Now
            };

            await _db.Expenses.AddAsync(expense);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = expense.Guid }, expense.Guid);
        }

        // PATCH api/expense/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateExpensePayload payload)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid expense id");

            var expense = await _db.Expenses.FirstOrDefaultAsync(e => e.Guid == validId);
            if (expense == null)
                return NotFound("Expense not found");

            if (payload.AmountSpent.HasValue && payload.AmountSpent > 0)
                expense.AmountSpent = payload.AmountSpent.Value;

            if (!string.IsNullOrWhiteSpace(payload.Description))
                expense.Description = payload.Description;

            if (payload.ExpenseDate.HasValue)
                expense.ExpenseDate = payload.ExpenseDate.Value;

            expense.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return Ok("Expense updated successfully");
        }

        // DELETE api/expense/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid expense id");

            var expense = await _db.Expenses.FirstOrDefaultAsync(e => e.Guid == validId);
            if (expense == null)
                return NotFound("Expense not found");

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
            return Ok("Expense deleted successfully");
        }
    }
}