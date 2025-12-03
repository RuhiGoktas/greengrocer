using System.Collections.Generic;
using System.Threading.Tasks;
using greengrocer.Models;

namespace greengrocer.Services
{
    public interface IOrderService
    {
        Task<List<OrderListItemDto>> GetAllAsync();
        Task<OrderListItemDto> CreateAsync(OrderCreateDto dto);
    }
}
