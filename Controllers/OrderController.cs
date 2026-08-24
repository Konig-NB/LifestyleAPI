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
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order is null)
                return NotFound(new { message = $"Order with id {id} was not found." });

            return Ok(order);
        }

        [HttpPost]
        [EnableRateLimiting("login")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null)
                return NotFound(new { message = $"Order with id {id} was not found." });

            return Ok(updated);
        }
    }
}