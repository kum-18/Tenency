using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tenency.Data;
using Tenency.Models;
using Tenency.Models.Dto;

namespace Tenency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly ApplicationDbcontext _db;

        public PropertyController(ApplicationDbcontext db)
        {
            _db = db;
        }

        // GET api/property
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertySummaryDto>>> GetAll()
        {
            var properties = await _db.Properties
                .Select(p => new PropertySummaryDto
                {
                    Guid = p.Guid,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    BaseRent = p.BaseRent,
                    TenantCount = p.Tenants.Count(t => t.EndDate == null)
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(properties);
        }

        // GET api/property/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDetailsDto>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid property id");

            var property = await _db.Properties
                .Where(p => p.Guid == validId)
                .Select(p => new PropertyDetailsDto
                {
                    Id = p.Guid,
                    PropertyName = p.PropertyName,
                    Address = p.Address,
                    LengthValue = p.LengthValue,
                    WidthValue = p.WidthValue,
                    Unit = p.Unit,
                    BaseRent = p.BaseRent,
                    BaseCurrentPrice = p.BaseCurrentPrice
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (property == null)
                return NotFound("Property not found");

            return Ok(property);
        }

        // POST api/property
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyPayload payload)
        {
            if (string.IsNullOrWhiteSpace(payload.PropertyName) ||
                string.IsNullOrWhiteSpace(payload.Address) ||
                payload.BaseRent <= 0 ||
                payload.BaseCurrentPrice <= 0)
            {
                return BadRequest("Fill all required fields correctly");
            }

            var property = new PropertyDetails
            {
                PropertyName = payload.PropertyName,
                Address = payload.Address,
                LengthValue = payload.LengthValue,
                WidthValue = payload.WidthValue,
                Unit = payload.Unit,
                BaseRent = payload.BaseRent,
                BaseCurrentPrice = payload.BaseCurrentPrice,
                CreatedAt = DateTime.Now
            };

            await _db.Properties.AddAsync(property);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = property.Guid }, property.Guid);
        }

        // PATCH api/property/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePropertyPayload payload)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid property id");

            var property = await _db.Properties.FirstOrDefaultAsync(p => p.Guid == validId);
            if (property == null)
                return NotFound("Property not found");

            // Only update fields that were provided
            if (!string.IsNullOrWhiteSpace(payload.PropertyName))
                property.PropertyName = payload.PropertyName;

            if (!string.IsNullOrWhiteSpace(payload.Address))
                property.Address = payload.Address;

            if (payload.BaseRent.HasValue && payload.BaseRent > 0)
                property.BaseRent = payload.BaseRent.Value;

            if (payload.BaseCurrentPrice.HasValue && payload.BaseCurrentPrice > 0)
                property.BaseCurrentPrice = payload.BaseCurrentPrice.Value;

            if (payload.LengthValue.HasValue)
                property.LengthValue = payload.LengthValue;

            if (payload.WidthValue.HasValue)
                property.WidthValue = payload.WidthValue;

            if (!string.IsNullOrWhiteSpace(payload.Unit))
                property.Unit = payload.Unit;
            property.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return Ok("Property updated successfully");
        }

        // DELETE api/property/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid validId))
                return BadRequest("Invalid property id");

            var property = await _db.Properties.FirstOrDefaultAsync(p => p.Guid == validId);
            if (property == null)
                return NotFound("Property not found");

            bool hasActiveTenants = await _db.Tenants
                .AnyAsync(t => t.PropertyId == validId && t.EndDate == null);

            if (hasActiveTenants)
                return Conflict("Cannot delete property with active tenants");

            _db.Properties.Remove(property);
            await _db.SaveChangesAsync();
            return Ok("Property deleted successfully");
        }
    }
}