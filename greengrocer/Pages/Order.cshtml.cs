using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using greengrocer.Data;
using greengrocer.Models;
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

        public OrderModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OrderInput Input { get; set; }

        public List<Order> ExistingOrders { get; set; }

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

        public void OnGet()
        {
            Input = new OrderInput
            {
                Items = new List<OrderItemInput>()
            };

            ExistingOrders = _db.Orders
                .Include(o => o.Items)
                .ToList();
        }

        public void OnPost()
        {

        }

        private OrderInput CreateEmptyOrder()
        {
            return new OrderInput
            {
                Items = new List<OrderItemInput>
                {
                    new OrderItemInput(),
                    new OrderItemInput(),
                    new OrderItemInput()
                }
            };
        }
    }
}
