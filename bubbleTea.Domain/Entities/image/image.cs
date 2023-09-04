namespace BubbleTea.Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public byte[] Data { get; set; } = Array.Empty<byte>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}