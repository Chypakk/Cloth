using Cloth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloth.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        private AddDbConnect Context;
        public OrderController(IOrderRepository orderRepository, Cart cartServices, AddDbConnect ctx)
        {
            repository = orderRepository;
            cart = cartServices;
            Context = ctx;
        }

        public IActionResult Checkout(int Promocode) 
        {
            ViewBag.Promocode = Promocode;
            return View(new Order());
        } 
        public IActionResult Completed()
        {
            cart.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order, int Promocode)
        {
            if (Promocode == 0)
            {
                order.TotalPrice = cart.ComputeTotalValue();
                order.UsingPromocode = false;
            }
            else
            {
                double percent = Promocode / 100.0;
                double price = cart.ComputeTotalValue() * percent;
                order.TotalPrice = cart.ComputeTotalValue() - price;
                order.UsingPromocode = true;
            }
            
            order.OrderDate = DateTime.Now;
            order.Name = User.Identity.Name;

            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Корзина пуста");
            }

            foreach (var item in cart.Lines)
            {
                Remains remains = Context.Remains.Where(a => a.ProductId == item.Product.Id && a.Size == item.Size).FirstOrDefault();
                if (remains != null)
                {
                    if (remains.Count >= item.Quantity)
                    {
                        remains.Count -= item.Quantity;
                        Context.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", $"К сожалению {item.Product.Name} {remains.Size} размера есть только {remains.Count} штук");
                    }
                }
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
