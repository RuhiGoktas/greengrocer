using System.Collections.Generic;

namespace greengrocer.Models
{
    public class OrderListItemDto
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public int TotalQty { get; set; }

        public string ItemsText { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }
}
