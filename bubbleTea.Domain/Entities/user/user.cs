using System.ComponentModel.DataAnnotations.Schema;
using BubbleTea.Domain.Enum;

namespace BubbleTea.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public Role Role { get; set; }

        public int LocationId { get; set; }
        public int ImageId { get; set; }

        public int PaymentMethodId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; } = new Location();

        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; } = new Image();

        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; set; } = new PaymentMethod();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}