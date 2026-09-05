using LifestyleAPI.DTOs;
using LifestyleAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace LifestyleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service) => _service = service;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest(new { message = "Page and pageSize must be greater than 0." });

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role);

            if (userIdClaim == null || roleClaim == null)
                return Unauthorized("Required claims are missing.");

            int userId = int.Parse(userIdClaim.Value);
            string role = roleClaim.Value;

            if (role == "Customer")
            {
                var result = await _service.GetAllForCustomerAsync(userId, page, pageSize);
                return Ok(result);
            }

            if (role == "Admin" || role == "Owner")
            {
                var result = await _service.GetAllAsync(page, pageSize);
                return Ok(result);
            }

            return Forbid();
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order is null)
                return NotFound(new { message = $"Order with id {id} was not found." });

            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            if (role == "Customer" && order.CustomerId != userId)
                return Forbid();

            return Ok(order);
        }

        [HttpPost]
        [EnableRateLimiting("login")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Required claims are missing." });

            int customerId = int.Parse(userIdClaim.Value);

            try
            {
                var created = await _service.CreateAsync(customerId, dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (updated is null)
                    return NotFound(new { message = $"Order with id {id} was not found." });

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}