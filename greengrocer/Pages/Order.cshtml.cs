using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using greengrocer.Data;
using greengrocer.Models;
using greengrocer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace greengrocer.Pages
{
    [Authorize]
    public class OrderModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IOrderService _orderService;


        public OrderModel(AppDbContext db, IOrderService orderService)
        {
            _db = db;
            _orderService = orderService;
        }

        [BindProperty]
        public OrderInput Input { get; set; }

        public List<OrderListItemDto> ExistingOrders { get; set; }

        public List<Stock> Stocks { get; set; }

        public string Message { get; set; }

        public class OrderItemInput
        {
            [Required]
            public string Title { get; set; }

            [Range(1, 9999)]
            public int Quantity { get; set; }
        }

        public class OrderInput
        {
            [Required]
            [Display(Name = "Order Name")]
            public string OrderName { get; set; }

            public List<OrderItemInput> Items { get; set; } = new List<OrderItemInput>();
        }

        public async Task OnGetAsync()
        {
          
            Input = new OrderInput
            {
                Items = new List<OrderItemInput>()
            };

            
            ExistingOrders = await _orderService.GetAllAsync(); 

            Stocks = await _db.Stocks
                .Where(s => s.IsActive)
                .OrderBy(s => s.StockId)
                .ToListAsync();
        }



       
    }
}
