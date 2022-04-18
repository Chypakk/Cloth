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

        public IActionResult Catalog(string search, string category, string brand, int productPage = 1)
        {
            ViewBag.SearchString = search;
            var result = new ProductsListViewModel
            {
                Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
                .OrderBy(p => p.Id)
                .Where(p => category == null || p.Categories.Name == category)
                .Where(p => brand == null || p.Brands.Name == brand)
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
                CurrentBrand = brand
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
            }
            else
            if (category != null)
            {
                result.PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Context.Products.Include(p => p.Categories).Where(p => p.Categories.Name == category).Count()
                };
            }
            else
            if (brand != null)
            {
                result.PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Context.Products.Include(p => p.Brands).Where(p => p.Brands.Name == brand).Count()
                };
            }

            return View(result);
        }

        public IActionResult ProductCard(int Id, int OptionsId, string Name)
        {
            var result = new ProductCardViewModel
            {
                Products = Context.Products.Include(a => a.Brands).Include(a => a.Categories)
                    .Where(a => a.Id == Id).FirstOrDefault(),
                Options = Context.Options.Where(a => a.Id == OptionsId).FirstOrDefault(),
                Picture = Context.Pictures.Where(a => a.Name == Name),
                ProductId = Id,
                Commentaries = Context.Commentaries.Where(a => a.ProductId == Id),
                UserName = User.Identity.Name,
            };
            return View(result);

        }
    }
}
