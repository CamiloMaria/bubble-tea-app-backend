using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTea.Domain.Entities
{
    public class ProductSize
    {
        public int ProductId { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = new Product();

        public int SizeId { get; set; }

        [ForeignKey(nameof(SizeId))]
        public Size Size { get; set; } = new Size();
    }
}