using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cloth.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        private DataContext Context;
        private UserManager<AppUser> userManager;

        public OrderController(IOrderRepository orderRepository, Cart cartServices, DataContext ctx, UserManager<AppUser> usr)
        {
            repository = orderRepository;
            cart = cartServices;
            Context = ctx;
            userManager = usr;
        }

        public async Task<IActionResult> Checkout(string? Promocodes)
        {
            if (Promocodes != null)
            {
                Promocode promocode = Context.Promocodes.FirstOrDefault(a => a.Code.ToUpper() == Promocodes.ToUpper());
                if (promocode != null)
                {
                    if (DateTime.Now >= promocode.StartDate && DateTime.Now <= promocode.EndDate)
                    {
                        ViewBag.PromocodeMessage = "Промокод действителен";
                        ViewBag.Promocode = promocode.Percent;
                    }
                    else
                    {
                        ViewBag.PromocodeMessage = "Действие промокода закончилось";
                    }
                }
                else
                {
                    ViewBag.PromocodeMessage = "Такого промокода не существует";
                }
            }
            var user = userManager.Users.FirstOrDefault(a => a.UserName == User.Identity.Name);
            var order = new Order
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Adress = user.Adress,
                City = user.City,
                Country = user.Country,
                Index = user.Index,
            };

            //ViewBag.Promocode = promocode;
            return View(order);
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
                order.PromocodePercent = Promocode;
            }
            order.OrderDate = DateTime.Now;
            order.Name = User.Identity.Name;
            order.Status = "Ожидание";
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Корзина пуста");
            }            
            if (ModelState.IsValid)
            {
                foreach (var item in cart.Lines)
                {
                    Remains remains = Context.Remains.Where(a => a.ProductId == item.Product.Id && a.Size == item.Size).FirstOrDefault();
                    if (remains != null)
                    {
                        if (remains.Count >= item.Quantity)
                        {
                            remains.Count -= item.Quantity;
                        }
                        else
                        {
                            ModelState.AddModelError("", $"К сожалению {item.Product.Name} {remains.Size} размера есть только {remains.Count} штук");
                            return View(order);
                        }
                    }
                }
                Context.SaveChanges();
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
