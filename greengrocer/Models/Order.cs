using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace greengrocer.Models
{
    [Table("Order")]    
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public int DeliveryAddressId { get; set; }
        public int InvoiceAddressId { get; set; }
        public bool IsActive { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
