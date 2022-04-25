using Cloth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloth.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        public OrderController(IOrderRepository orderRepository, Cart cartServices)
        {
            repository = orderRepository;
            cart = cartServices;
        }

        public IActionResult Checkout() => View(new Order());
        public IActionResult Completed()
        {
            cart.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            order.Name = User.Identity.Name;
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Корзина пуста");
            }
            
            if (ModelState.IsValid) 
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
    }
}
