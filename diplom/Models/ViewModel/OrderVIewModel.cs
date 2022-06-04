namespace Cloth.Models.ViewModel
{
    public class OrderVIewModel
    {
        public AppUser User { get; set; }
        public Order Order { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Index { get; set; }
    }
}
