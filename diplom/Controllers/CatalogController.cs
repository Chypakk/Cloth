using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloth.Controllers
{
    public class CatalogController : Controller
    {

        private AddDbConnect Context;
        private int PageSize = 2;

        public CatalogController(AddDbConnect ctx)
        {
           Context = ctx;
        }

        public IActionResult Catalog(string Search, string category, int productPage = 1)
        {
            ViewBag.SearchString = Search;
            var result = new ProductsListViewModel
            {
                Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
                .OrderBy(p => p.Id)
                .Where(p =>
                    (Search == null || p.Name.ToLower().Contains(Search.ToLower())
                    || p.Categories.Name.ToLower().Contains(Search.ToLower())
                    || p.Brands.Name.ToLower().Contains(Search.ToLower())) &&
                    (category == null || p.Categories.Name == category))
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        Context.Products.Where(p =>
                            (Search == null || p.Name.ToLower().Contains(Search.ToLower())
                            || p.Categories.Name.ToLower().Contains(Search.ToLower())
                            || p.Brands.Name.ToLower().Contains(Search.ToLower())))
                            .Count() :
                        Context.Products.Include(a => a.Categories).Where(a => a.Categories.Name == category).Count()
                },
                CurrentCategory = category
            };
            return View(result);
        }
    }
}
