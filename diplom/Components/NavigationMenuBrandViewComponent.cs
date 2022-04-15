﻿using Cloth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Components
{
    public class NavigationMenuBrandViewComponent : ViewComponent
    {
        private AddDbConnect context;
        public NavigationMenuBrandViewComponent(AddDbConnect ctx)
        {
            context = ctx;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedBrand = RouteData?.Values["brand"];
            return View(context.Products
                .Include(ca => ca.Brands)
                .Select(c => c.Brands.Name)
                .Distinct()
                .OrderBy(a => a)
                );
        }
    }
}
