using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = new User();
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Reference { get; set; }
        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = new City();
        public string State { get; set; } = string.Empty;
        public string? Zip { get; set; }
    }
}