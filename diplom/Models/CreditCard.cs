namespace Cloth.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public int CardCode { get; set; }
        public DateTime DateCard { get; set; }

        public ClientsData ClientsData { get; set; }
        public List<Order> Orders { get; set; }
    }
}
