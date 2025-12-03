using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using greengrocer.Models;
using greengrocer.Repositories;

namespace greengrocer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<OrderListItemDto>> GetAllAsync()
        {
            var orders = await _repo.GetAllWithItemsAsync();

            return orders.Select(o => new OrderListItemDto
            {
                Id = o.Id,
                OrderName = o.OrderName,
                TotalQty = o.Items.Sum(i => i.Quantity),
                ItemsText = string.Join(", ", o.Items.Select(i => i.Title + " (x" + i.Quantity + ")")),
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Title = i.Title,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<OrderListItemDto> CreateAsync(OrderCreateDto dto)
        {
            var order = new Order
            {
                OrderName = dto.OrderName,
                Items = dto.Items
                    .Where(i => !string.IsNullOrWhiteSpace(i.Title) && i.Quantity > 0)
                    .Select(i => new OrderItem
                    {
                        Title = i.Title,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };

            var saved = await _repo.AddAsync(order);

            return new OrderListItemDto
            {
                Id = saved.Id,
                OrderName = saved.OrderName,
                TotalQty = saved.Items.Sum(i => i.Quantity),
                ItemsText = string.Join(", ", saved.Items.Select(i => i.Title + " (x" + i.Quantity + ")")),
                Items = saved.Items.Select(i => new OrderItemDto
                {
                    Title = i.Title,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
