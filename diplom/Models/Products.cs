namespace Cloth.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public double Rating { get; set; }
        public byte[] ProductImage { get; set; }
        //public int RemainsId { get; set; }

        //public int Quantity { get; set; }

        public Brand Brands { get; set; }
        public Category Categories { get; set; }
        //public List<Order> Orders { get; set; }
        //public Remains Remains { get; set; }
    }
}
