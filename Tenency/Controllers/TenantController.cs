using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tenency.Data;
using Tenency.Models;
using Tenency.Models.Dto;

namespace Tenency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;

        public TenantController(ApplicationDbcontext db)
        {
            _db = db;
        }

        // GET api/tenant?propertyId={propertyId}
        // GET api/tenant (all tenants across all properties)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantSummaryDto>>> GetAll(
            [FromQuery] string? propertyId)
        {
            var query = _db.Tenants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(propertyId))
            {
                if (!Guid.TryParse(propertyId, out Guid validPropertyId))
                    return BadRequest("Invalid propertyId");

                query = query.Where(t => t.PropertyId == validPropertyId);
            }

            var tenants = await query
                .Select(t => new TenantSummaryDto
                {
                    Id = t.Guid,
                    TenantName = t.TenantName,
                    RentType = t.RentType,
                    ShopName = t.ShopName,
                    PropertyName = t.Property.PropertyName,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    IsActive = t.EndDate == null
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(tenants);
        }

        // GET api/tenant/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantDetailsDto>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid tenant id");

            var tenant = await _db.Tenants
                .Where(t => t.Guid == validId)
                .Select(t => new TenantDetailsDto
                {
                    Id = t.Guid,
                    TenantName = t.TenantName,
                    ProofNumber = t.ProofNumber,
                    RentType = t.RentType,
                    ShopName = t.ShopName,
                    PropertyName = t.Property.PropertyName,
                    PropertyId = t.PropertyId,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    IsActive = t.EndDate == null
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (tenant == null)
                return NotFound("Tenant not found");

            return Ok(tenant);
        }

        // POST api/tenant
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantPayload payload)
        {
            if (string.IsNullOrWhiteSpace(payload.PropertyId) ||
                string.IsNullOrWhiteSpace(payload.TenantName) ||
                string.IsNullOrWhiteSpace(payload.ProofNumber) ||
                string.IsNullOrWhiteSpace(payload.RentType))
            {
                return BadRequest("Fill all required fields correctly");
            }

            if (!Guid.TryParse(payload.PropertyId, out Guid validPropertyId))
                return BadRequest("Invalid propertyId");

            var validRentTypes = new[] { "commercial", "house" };
            if (!validRentTypes.Contains(payload.RentType.ToLower()))
                return BadRequest("RentType must be 'commercial' or 'house'");

            var property = await _db.Properties.FirstOrDefaultAsync(p => p.Guid == validPropertyId);
            if (property == null)
                return NotFound("Property not found");

            var tenant = new TenentDetails
            {
                PropertyId = validPropertyId,
                TenantName = payload.TenantName,
                ProofNumber = payload.ProofNumber,
                RentType = payload.RentType.ToLower(),
                ShopName = payload.ShopName,
                StartDate = payload.StartDate,
                CreatedAt = DateTime.Now
            };

            await _db.Tenants.AddAsync(tenant);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tenant.Guid }, tenant.Guid);
        }

        // PATCH api/tenant/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTenantPayload payload)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid tenant id");

            var tenant = await _db.Tenants.FirstOrDefaultAsync(t => t.Guid == validId);
            if (tenant == null)
                return NotFound("Tenant not found");

            if (!string.IsNullOrWhiteSpace(payload.TenantName))
                tenant.TenantName = payload.TenantName;

            if (!string.IsNullOrWhiteSpace(payload.ShopName))
                tenant.ShopName = payload.ShopName;

            if (!string.IsNullOrWhiteSpace(payload.ProofNumber))
                tenant.ProofNumber = payload.ProofNumber;

            // Vacating the tenant
            if (payload.EndDate.HasValue)
            {
                if (payload.EndDate < tenant.StartDate)
                    return BadRequest("End date cannot be before start date");

                tenant.EndDate = payload.EndDate;
            }
            tenant.UpdatedAt = DateTime.Now
            await _db.SaveChangesAsync();
            return Ok("Tenant updated successfully");
        }

        // DELETE api/tenant/{id}
        // Soft delete — just sets EndDate to today
        [HttpDelete("{id}")]
        public async Task<IActionResult> Vacate(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid tenant id");

            var tenant = await _db.Tenants.FirstOrDefaultAsync(t => t.Guid == validId);
            if (tenant == null)
                return NotFound("Tenant not found");

            if (tenant.EndDate != null)
                return Conflict("Tenant has already vacated");

            tenant.EndDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return Ok("Tenant marked as vacated");
        }
    }
}