using System.ComponentModel.DataAnnotations;

namespace greengrocer.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public int Quantity { get; set; }

        // FK
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
