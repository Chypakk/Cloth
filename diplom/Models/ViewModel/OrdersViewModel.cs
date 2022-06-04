namespace Cloth.Models.ViewModel
{
    public class OrdersViewModel
    {
        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<CartLine> CartLines { get; set; }
    }
}
