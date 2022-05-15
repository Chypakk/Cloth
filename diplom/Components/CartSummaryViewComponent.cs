using Cloth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloth.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;
        public CartSummaryViewComponent(Cart cartServices) { cart = cartServices; }

        public IViewComponentResult Invoke() => View(cart);
    }
}
