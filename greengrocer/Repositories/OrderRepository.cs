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
            return _db.Orders
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<Order> AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }
    }
}
