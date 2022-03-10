namespace Cloth.Models
{
    public class Remains
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        //public int ProductId { get; set; }
        public int Count { get; set; }

        //public List<Products> Products { get; set; }
        public List<Warehouse> Warehouse { get; set; }
    }
}
