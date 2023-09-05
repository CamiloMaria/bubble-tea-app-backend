using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class AuthToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = new User();
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}