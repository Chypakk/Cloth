using Cloth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private AddDbConnect context;
        public NavigationMenuViewComponent(AddDbConnect ctx)
        {
            context = ctx;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(context.Products
                .Include(ca => ca.Categories)
                .Select(c => c.Categories.Name)
                .Distinct()
                .OrderBy(a => a)
                );
        }
    }
}
