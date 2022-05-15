namespace Cloth.Models
{
    public class Options
    {
        public int Id { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Design { get; set; }
        public string Description { get; set; }
        public string CareNote { get; set; }


        public List<Products> Products { get; set; }
    }
}
