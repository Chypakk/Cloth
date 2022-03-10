namespace Cloth.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Products> Products { get; set; }
    }
}
