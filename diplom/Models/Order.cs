namespace Cloth.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CardId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? Promocode { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }

        public ClientsData ClientsData { get; set; }
        public CreditCard CreditCard { get; set; }
        public List<Products> Products { get; set; }
    }
}
