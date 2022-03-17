namespace Cloth.Models
{
    public class Deliveries
    {
        public int Id { get; set; }
        public string DeliverName { get; set; }
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        //public Products Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
