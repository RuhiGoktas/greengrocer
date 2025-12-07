using System.Collections.Generic;
using System.Threading.Tasks;
using greengrocer.Models;

namespace greengrocer.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllWithItemsAsync();
        Task<Order> AddAsync(Order order);
        Task<string> GetNextOrderNoAsync();
    }
}
