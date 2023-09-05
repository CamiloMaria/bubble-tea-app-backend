using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTea.Domain.Enum;

namespace BubbleTea.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; private set; } 

        public DateTime OrderDate { get; } = DateTime.Now;

        public DateTime CompletedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = new User();

        public Order()
        {
            Status = OrderStatus.Pending;
            CalculateTotalPrice();
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = OrderItems.Sum(x => x.Product.Price * x.Quantity);
        }
    }
}