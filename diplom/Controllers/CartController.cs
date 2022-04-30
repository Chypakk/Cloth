using Cloth.Infrastructure;
using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Controllers
{
    public class CartController : Controller
    {
        private AddDbConnect context;
        private Cart cart;
        public CartController(AddDbConnect ctx, Cart cartServices) { 
            context = ctx;
            cart = cartServices;
        }

        public RedirectToActionResult AddToCart(int Id, string returnUrl, int Quantity, int Size)
        {
            //Remains exp = context.Remains.Include(a => a.Products).FirstOrDefault(a => a.ProductId == Id);
            Products product = context.Products.FirstOrDefault(p => p.Id == Id);
            if (Quantity == 0)
            {
                Quantity++;
            }
            if (product != null)
            {
                cart.AddItem(product, Quantity, Size);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Products product = context.Products.FirstOrDefault(p => p.Id == Id);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl,
            }) ;
        }

    }
}
