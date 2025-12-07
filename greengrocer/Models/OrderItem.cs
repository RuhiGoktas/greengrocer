using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace greengrocer.Models
{
    [Table("OrderItems")]   
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }      

        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public string Title { get; set; }
        public int Quantity { get; set; }
    }
}
