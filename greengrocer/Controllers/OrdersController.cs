using System.Threading.Tasks;
using greengrocer.Models;
using greengrocer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace greengrocer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        // GET: /api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // POST: /api/orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
    }
}
