using LifestyleAPI.DTOs;
using LifestyleAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LifestyleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _service;

        public OrderItemController(IOrderItemService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest(new { message = "Page and pageSize must be greater than 0." });

            var result = await _service.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order is null)
                return NotFound(new { message = $"Order Item with id {id} was not found." });

            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemDTO dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
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
    }
}