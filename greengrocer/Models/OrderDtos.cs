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

    public class OrderCreateDto
    {
        [Required]
        public string OrderName { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
