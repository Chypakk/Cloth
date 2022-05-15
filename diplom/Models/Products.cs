namespace Cloth.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int OptionsId { get; set; }
        public int Price { get; set; }
        public byte[] ProductImage { get; set; }

        public Brand Brands { get; set; }
        public Category Categories { get; set; }
        public Options Options { get; set; }
        public List<Commentaries> Commentaries { get; set; }
        public List<Remains> Remains { get; set; }
    }
}
