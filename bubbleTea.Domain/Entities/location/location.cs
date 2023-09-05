using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = new User();
        public string Name { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double Latitude { get; set; }
        public string? Address { get; set; }
        public string? Reference { get; set; }
        public string? Zip { get; set; }
    }
}