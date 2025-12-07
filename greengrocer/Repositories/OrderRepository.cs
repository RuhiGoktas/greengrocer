using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using greengrocer.Data;
using greengrocer.Models;
using Microsoft.EntityFrameworkCore;

namespace greengrocer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Order>> GetAllWithItemsAsync()
        {
            return _db.Orders        // DbSet<Order> Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderId)
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<string> GetNextOrderNoAsync()
        {
            var last = await _db.Orders
                .OrderByDescending(o => o.OrderId)
                .FirstOrDefaultAsync();

            var number = 1;

            if (last != null && !string.IsNullOrEmpty(last.OrderNo) && last.OrderNo.StartsWith("ORD-"))
            {
                var part = last.OrderNo.Substring(4);
                int.TryParse(part, out number);
                number++;
            }

            return $"ORD-{number:0000}";
        }
    }

}
