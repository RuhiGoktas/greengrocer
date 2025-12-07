using System;
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
                OrderId = o.OrderId,
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                ItemsText = string.Join(", ", o.Items.Select(i => $"{i.Title} (x{i.Quantity})")),
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Title = i.Title,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<OrderListItemDto> CreateAsync(OrderCreateDto dto)
        {
                if (dto.Items == null || !dto.Items.Any())
                    throw new ArgumentException("En az bir kalem olmalı");

                var orderNo = await _repo.GetNextOrderNoAsync();

                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    DeliveryAddressId = dto.DeliveryAddressId,
                    InvoiceAddressId = dto.InvoiceAddressId,
                    OrderDate = DateTime.Now,
                    OrderNo = orderNo,
                    IsActive = true
                };

                order.Items = dto.Items
                    .Where(i => !string.IsNullOrWhiteSpace(i.Title) && i.Quantity > 0)
                    .Select(i => new OrderItem
                    {
                        Title = i.Title,
                        Quantity = i.Quantity
                    }).ToList();

                var saved = await _repo.AddAsync(order);   // _db.Orders.Add(order) + SaveChanges


            return new OrderListItemDto
            {
                OrderId = saved.OrderId,
                OrderNo = saved.OrderNo,
                OrderDate = saved.OrderDate,
                TotalPrice = saved.TotalPrice,
                ItemsText = string.Join(", ", saved.Items.Select(i => $"{i.Title} (x{i.Quantity})")),
                Items = saved.Items.Select(i => new OrderItemDto
                {
                    Title = i.Title,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }

}
