namespace BubbleTea.Domain.Entities
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}