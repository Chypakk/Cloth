using Microsoft.EntityFrameworkCore;

namespace Cloth.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private DataContext context;
        public EFOrderRepository(DataContext ctx) { context = ctx; }

        public IQueryable<Order> Orders => context.Orders.Include(a => a.Lines).ThenInclude(a => a.Product);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(a => a.Product));
            if (order.Id == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
