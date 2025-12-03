using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace greengrocer.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string OrderName { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
