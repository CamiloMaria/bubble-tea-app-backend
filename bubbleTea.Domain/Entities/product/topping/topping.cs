namespace BubbleTea.Domain.Entities
{
    public class Topping
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ProductTopping> ProductToppings { get; set; } = new List<ProductTopping>();
    }
}