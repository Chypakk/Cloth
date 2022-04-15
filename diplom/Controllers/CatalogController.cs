using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Controllers
{
    public class CatalogController : Controller
    {

        private AddDbConnect Context;
        private int PageSize = 8;

        public CatalogController(AddDbConnect ctx)
        {
            Context = ctx;
        }

        public IActionResult Catalog(string search, string category, string brands, int productPage = 1)
        {
            ViewBag.SearchString = search;
            var result = new ProductsListViewModel
            {
                Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
                .OrderBy(p => p.Id)
                .Where(p => category == null || p.Categories.Name == category)
                .Where(p => brands == null || p.Brands.Name == brands)
                .Where(p =>
                    (search == null || p.Name.ToLower().Contains(search.ToLower())
                    || p.Categories.Name.ToLower().Contains(search.ToLower())
                    || p.Brands.Name.ToLower().Contains(search.ToLower()))
                    )
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Context.Products.Count()
                },
                
                CurrentCategory = category,
                Search = search,
                CurrentBrand = brands
            };

            if (search != null)
            {
                result.PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Context.Products.Where(p =>
                        p.Name.Contains(search.ToLower()) ||
                        p.Categories.Name.ToLower().Contains(search.ToLower()) ||
                        p.Brands.Name.ToLower().Contains(search.ToLower())
                    ).Count()
                };
            } else
            if (category != null)
            {
                result.PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Context.Products.Include(p => p.Categories).Where(p => p.Categories.Name == category).Count()
                };
            } else
            if (brands != null)
            {
                result.PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Context.Products.Include(p => p.Brands).Where(p => p.Brands.Name == brands).Count()
                };
            }

            return View(result);
        }

        public IActionResult ProductCard() => View();
    }
}
