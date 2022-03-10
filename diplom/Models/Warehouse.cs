namespace Cloth.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }

        public List<Deliveries> Deliveries { get; set; }
        public Remains Remains { get; set; }
    }
}
