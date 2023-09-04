using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StateId { get; set; }

        [ForeignKey(nameof(StateId))]
        public State State { get; set; } = new State();
        public ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}