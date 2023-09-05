using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public DateTime CreatedDate { get; } = DateTime.Now;

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = new Order();

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = new Product();
    }
}