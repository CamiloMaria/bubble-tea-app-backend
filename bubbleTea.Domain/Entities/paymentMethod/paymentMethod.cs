using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTea.Domain.Enum;

namespace BubbleTea.Domain.Entities
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        public int? OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CardNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public string? SecurityCode { get; set; }
        public string? CardHolderName { get; set; }
        public string? CardHolderBirthDate { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public PaymentMethod()
        {
            PaymentType = PaymentType.Cash;
        }
    }
}