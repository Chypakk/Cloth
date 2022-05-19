using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Controllers
{
    public class CatalogController : Controller
    {

        private DataContext Context;
        private int PageSize = 8;

        public CatalogController(DataContext ctx)
        {
            Context = ctx;
        }

        public IActionResult Catalog(string search, string category, string brand, int minPrice, int maxPrice, int productPage = 1)
        {
            ViewBag.SearchString = search;
            Console.WriteLine($"MINAMAL PRICE = {minPrice} AND MAXIMAL PRICE {maxPrice}");
            var result = new ProductsListViewModel
            {
                Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
                .OrderBy(p => p.Id)

                .Where(p => minPrice == 0 || p.Price >= minPrice)
                .Where(p => maxPrice == 0 || p.Price <= maxPrice)
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
                CurrentBrand = brand,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
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

        public IActionResult ProductCard(int ComId, string ComName)
        {
            ViewBag.Id = ComId;
            ViewBag.Name = ComName;
            
            var result = new ProductCardViewModel
            {
                Products = Context.Products.Include(a => a.Brands).Include(a => a.Categories).Include(a => a.Options).Include(a => a.Remains)
                    .FirstOrDefault(a => a.Id == ComId),
                Picture = Context.Pictures.Where(a => a.Name == ComName),
                ProductId = ComId,
                Commentaries = Context.Commentaries.Where(a => a.ProductId == ComId),
                UserName = User.Identity.Name,
            };
            return View(result);

        }
    }
}
