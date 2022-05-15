using Cloth.Infrastructure;
using Newtonsoft.Json;

namespace Cloth.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        
        public ISession Session { get; set; }
        public override void AddItem(Products product, int quantity, string size)
        {
            base.AddItem(product, quantity, size);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Products product, string size)
        {
            base.RemoveLine(product, size);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
