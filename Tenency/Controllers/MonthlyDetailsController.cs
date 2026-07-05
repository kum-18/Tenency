using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tenency.Data;
using Tenency.Models;
using Tenency.Models.Dto;

namespace Tenency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyDetailsController : ControllerBase
    {
        private ApplicationDbcontext TenancyDbContext;
        public MonthlyDetailsController(ApplicationDbcontext dbcontext)
        {
            TenancyDbContext = dbcontext;
        }
        [HttpPost("monthlydetails}")]
        public async Task<IActionResult> SaveMonthlyDetails([FromQuery]string tenantId, [FromQuery]decimal newReading)
        {
            bool isValid = Guid.TryParse(tenantId, out Guid validTenantId);
            if(!isValid)
            {
                return BadRequest("Invalid tenantId");
            }
            var tenant = await TenancyDbContext.Tenants.Include(t=>t.Property).FirstOrDefaultAsync((tent)=>tent.Guid==validTenantId);
            if(tenant == null)
            {
                return BadRequest("Not a valid Tenant");
            }
            if(newReading <= 0)
            {
                return BadRequest("Need a valid Reading");
            }


            var todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
            var billingMonth = new DateOnly(todayDateOnly.Year, todayDateOnly.Month, 1);

            // Duplicate bill guard
            var existingBill = await TenancyDbContext.MonthlyDetails
                .AnyAsync(m => m.TenantId == validTenantId && m.BillingMonth == billingMonth);
            if (existingBill)
                return Conflict("Bill already generated for this month");

            var lastMonthDetails = await TenancyDbContext.MonthlyDetails
                .AsNoTracking()
                .Where(m => m.TenantId == validTenantId && m.BillingMonth < billingMonth)
                .OrderByDescending(m => m.BillingMonth)
                .FirstOrDefaultAsync();

            bool isFirstBill = lastMonthDetails == null;

            // Reading baseline
            decimal readingFrom = isFirstBill
                ? tenant.StartingCurrent
                : lastMonthDetails!.CurrentReadingTo;
            decimal currentUsed = newReading - readingFrom;
            decimal baseRent;
            if (isFirstBill && tenant.MoveInDate.Day != 1)
            {
                int daysInMonth = DateTime.DaysInMonth(billingMonth.Year, billingMonth.Month);
                int daysOccupied = daysInMonth - tenant.MoveInDate.Day + 1;
                baseRent = Math.Round(tenant.Property.BaseRent / daysInMonth * daysOccupied, 2);

                // Lock in rent_start_date as this billing month's 1st
                var trackedTenant = await TenancyDbContext.Tenants.FindAsync(validTenantId);
                trackedTenant!.RentStartDate = billingMonth;
                trackedTenant.UpdatedAt = DateTime.Now;
            }
            else
            {
                baseRent = tenant.Property.BaseRent; // full fixed rent every month
            }

            decimal electricityCharge = currentUsed * tenant.Property.BaseCurrentPrice;
            decimal previousDue = lastMonthDetails?.Due ?? 0;
            decimal totalMonthlyRent = baseRent + electricityCharge + previousDue;

            var monthlyDetails = new MonthlyDetails
            {
                TenantId = tenant.Guid,
                PropertyId = tenant.PropertyId,
                BillingMonth = billingMonth,
                CurrentReadingFrom = readingFrom,
                CurrentReadingTo = newReading,
                CurrentUsed = currentUsed,
                ElectricityCharges = electricityCharge,
                TotalMonthlyRent = totalMonthlyRent,
                Due = totalMonthlyRent,   // starts as full amount; cleared when tenant pays
                CreatedAt = DateTime.Now
            };

            await TenancyDbContext.MonthlyDetails.AddAsync(monthlyDetails);

            try
            {
                await TenancyDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, "Monthly details saved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save monthly details");
            }
        }

        [HttpPatch("updateDue")]
        public async Task<IActionResult> UpdateMonthlyDue([FromBody] UpdateDuePayload updateDuePayload)
        {
            if(updateDuePayload.tenantId == null ||  updateDuePayload.AmountPaid == null || updateDuePayload.AmountPaid <= 0)
            {
                return BadRequest("Fill all the required fields correctly");
            }
            bool isValid = Guid.TryParse(updateDuePayload.tenantId, out Guid validTenantId);
            if (!isValid)
            {
                return BadRequest("Invalid tenantId");
            }
            var Tenant = await TenancyDbContext.Tenants.FirstOrDefaultAsync((tent) => tent.Guid == validTenantId);
            if (Tenant == null)
            {
                return NotFound("Not a valid Tenant");
            }

            var todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
            var billingMonth = new DateOnly(todayDateOnly.Year, todayDateOnly.Month, 1);
            var currentMonthDetails = await TenancyDbContext.MonthlyDetails
                .Where(m => m.TenantId == validTenantId &&  m.BillingMonth == billingMonth)
                .OrderByDescending(m => m.BillingMonth)
                .FirstOrDefaultAsync();
            if(currentMonthDetails == null)
            {
                return NotFound("current month bill is not generated yet");
            }

            currentMonthDetails.AmountPaid += updateDuePayload.AmountPaid;
            decimal Due = Math.Max(0,currentMonthDetails.Due - updateDuePayload.AmountPaid);
            currentMonthDetails.Due = Due;
            currentMonthDetails.UpdatedAt = DateTime.Now;
            try
            {
                await TenancyDbContext.SaveChangesAsync();
            }
            catch(DBConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user details. The Email or Phone Number may already be in use.");

            }
            return Ok("Sucessfully updated");
        }

        [HttpGet("{tenantId}")]
        public async Task<ActionResult<MonthlyDetailsCollection[]>> GetMonthlyDetails(string tenantId)
        {
            bool isValid = Guid.TryParse(tenantId, out Guid validTenantId);
            if (!isValid)
            {
                return BadRequest("Invalid tenantId");
            }
            var Tenant = await TenancyDbContext.Tenants.FirstOrDefaultAsync((tent) => tent.Guid == validTenantId);
            if (Tenant == null)
            {
                return BadRequest("Not a valid Tenant");
            }
            var currentMonthDetails = await TenancyDbContext.MonthlyDetails
                .Where(m => m.TenantId == validTenantId)
                .Select(m=> new MonthlyDetailsCollection
                {
                    Guid=m.Guid,
                    BillingMonth= m.BillingMonth,
                    PropertyName = m.Property.PropertyName,
                    AmountPaid = m.AmountPaid,
                    TotalMonthlyRent = m.TotalMonthlyRent,
                })
                .OrderByDescending(m => m.BillingMonth)
                .AsNoTracking().ToListAsync();

            return Ok(currentMonthDetails);

        }

        [HttpGet("{tenantId}/{monthDetailsId}")]
        public async Task<ActionResult<MonthlyDetailsDto>> GetMonthlyDetailsById(string monthDetailsId, string tenantId)
        {
            bool isValid = Guid.TryParse(tenantId, out Guid validTenantId);
            bool isValidMonthlyId = Guid.TryParse(monthDetailsId, out Guid validMonthDetailsId);
            if (!isValid || !isValidMonthlyId)
            {
                return BadRequest("Invalid tenantId");
            }
            var Tenant = await TenancyDbContext.Tenants.Where((tent) => tent.Guid == validTenantId).Include(t=>t.Property).FirstOrDefaultAsync();
            if (Tenant == null)
            {
                return NotFound("Not a valid Tenant");
            }
            var MonthlyDetails = await TenancyDbContext.MonthlyDetails.Where((mon) => mon.Guid == validMonthDetailsId).Select(monDet => new MonthlyDetailsDto
            {
                Id = monDet.Guid,
                AmountPaid = monDet.AmountPaid,
                BillingMonth = monDet.BillingMonth,
                CurrentReadingFrom = monDet.CurrentReadingFrom,
                CurrentReadingTo = monDet.CurrentReadingTo,
                Due = monDet.Due,
                ElectricityCharges = monDet.ElectricityCharges,
                BaseRent = Tenant.Property.BaseRent,
                BaseCurrentCharges = Tenant.Property.BaseCurrentPrice,
                CurrentUsed = monDet.CurrentUsed,
                PropertyName = monDet.Property.PropertyName,
                TotalMonthlyRent = monDet.TotalMonthlyRent,
                CreatedAt = monDet.CreatedAt,
                UpdatedAt = monDet.UpdatedAt ?? null
            }).AsNoTracking().FirstOrDefaultAsync();
            if(MonthlyDetails == null)
            {
                return NotFound("Not valid monthly id");
            }
            return Ok(MonthlyDetails);

        }
    }
}
