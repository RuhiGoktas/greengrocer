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

        // ViewModel for the form
        [BindProperty]
        public OrderInput Input { get; set; }

        // Orders from DB
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
            // start with an empty order (no rows yet)
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
            ExistingOrders = _db.Orders
                .Include(o => o.Items)
                .ToList();

            if (!ModelState.IsValid)
            {
                return;
            }

            // Map ViewModel to Entity
            var items = (Input.Items ?? new List<OrderItemInput>())
                .Where(i => !string.IsNullOrWhiteSpace(i.Title))
                .Select(i => new OrderItem
                {
                    Title = i.Title,
                    Quantity = i.Quantity
                })
                .ToList();

            var order = new Order
            {
                OrderName = Input.OrderName,
                Items = items
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            Message = "Order saved. Item count: " + order.Items.Count;

            // reload list including the newly saved order
            ExistingOrders = _db.Orders
                .Include(o => o.Items)
                .ToList();

            // reset form (empty again)
            Input = new OrderInput
            {
                Items = new List<OrderItemInput>()
            };
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
