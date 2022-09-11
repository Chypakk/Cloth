namespace Cloth.Models
{
    public class Remains
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }

        public Products Products { get; set; }
    }
}
