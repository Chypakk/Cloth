namespace Cloth.Models.ViewModel
{
    public class ProfileViewModel
    {

        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<CartLine> CartLines { get; set; }
        public AppUser User { get; set; }
        
    }
}
