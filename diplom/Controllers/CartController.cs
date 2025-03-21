﻿using Cloth.Infrastructure;
using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Controllers
{
    public class CartController : Controller
    {
        private DataContext context;
        private Cart cart;
        public CartController(DataContext ctx, Cart cartServices) { 
            context = ctx;
            cart = cartServices;
        }

        public RedirectToActionResult AddToCart(int Id, string returnUrl,
            int Quantity, string Size)
        {
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

        public RedirectToActionResult RemoveFromCart(int Id, string returnUrl, string Size)
        {
            Products product = context.Products.FirstOrDefault(p => p.Id == Id);
            if (product != null)
            {
                cart.RemoveLine(product, Size);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult Index(string returnUrl, string Promocodes, int flag)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl,
            }) ;
        }

        public IActionResult CheckPromocode(string Promocodes, int flag)
        {

           
            return RedirectToAction("Index", new {Promocodes, flag});
        }

    }
}
