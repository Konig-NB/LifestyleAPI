using LifestyleAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifestyleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        public NotificationController(INotificationService service) => _service = service;

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Required claims are missing." });

            int customerId = int.Parse(userIdClaim.Value);

            var result = await _service.GetAllForCustomerAsync(customerId);

            return Ok(result);
        }
    }
}