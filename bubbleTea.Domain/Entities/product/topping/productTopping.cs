using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class ProductTopping
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = new Product();

        public int ToppingId { get; set; }

        [ForeignKey(nameof(ToppingId))]
        public Topping Topping { get; set; } = new Topping();
    }
}