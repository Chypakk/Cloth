namespace Cloth.Models
{
    public class ClientsData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int CardId { get; set; }

        public List<CreditCard> CreditCard { get; set; }
        public List<Order> Orders { get; set; }
    }
}
