using System.Linq;
using greengrocer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greengrocer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /api/orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            var data = _db.Orders
                .Include(o => o.Items)
                .Select(o => new
                {
                    id = o.Id,
                    orderName = o.OrderName,
                    totalQty = o.Items.Sum(i => i.Quantity),

                    items = o.Items.Select(i => new { title = i.Title, quantity = i.Quantity }),
        
        itemsText = string.Join(
                        ", ",
                        o.Items.Select(i => i.Title + " (x" + i.Quantity + ")")
                    )
                })
                .ToList();


            return Ok(data);
        }
    }
}
