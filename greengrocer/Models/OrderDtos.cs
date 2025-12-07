using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace greengrocer.Models
{
    public class OrderItemDto
    {
        [Required]
        public string Title { get; set; }

        [Range(1, 9999)]
        public int Quantity { get; set; }
    }
    public class OrderListItemDto
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public string ItemsText { get; set; }   
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderCreateItemDto
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderCreateDto
    {
        // Şimdilik Customer / Address sabit de verilebilir
        public int CustomerId { get; set; } = 1;
        public int DeliveryAddressId { get; set; } = 1;
        public int InvoiceAddressId { get; set; } = 1;

        public List<OrderCreateItemDto> Items { get; set; } 
    }

}
