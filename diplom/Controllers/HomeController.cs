using Cloth.Models.ViewModel;
using Cloth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cloth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private int PageSize = 1;
        private CookieOptions opt = new CookieOptions();

        public AddDbConnect Context { get; set; }
        public ProductsListViewModel productsListViewModel { get; set; }
        public HomeController(ILogger<HomeController> logger, AddDbConnect ctx)
        {
            _logger = logger;
            Context = ctx;
        }

        public IActionResult Index() => View();
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public IActionResult Catalog(string? Search, string category, int productPage = 1)
        {
            if (Search != null)
            {
                return View(new ProductsListViewModel
                {
                    Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
                    .OrderBy(p => p.Id)
                    .Where(p => (p.Name.ToLower().Contains(Search.ToLower()) || p.Categories.Name.ToLower().Contains(Search.ToLower()) || p.Brands.Name.ToLower().Contains(Search.ToLower())) &&
                        (category == null || p.Categories.Name == category)),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = Context.Products.Count(),
                        TotalItems = category == null ?
                            Context.Products.Count() :
                            Context.Products.Include(a => a.Categories).Where(a => a.Categories.Name == category).Count()
                    },
                    SearchString = Search,
                    CurrentCategory = category
                });
            }
            else
                return View(new ProductsListViewModel
                {
                    Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
                    .OrderBy(a => a.Id)
                    .Where(p => category == null || p.Categories.Name == category)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = category == null ?
                        Context.Products.Count() :
                        Context.Products.Include(a => a.Categories).Where(a => a.Categories.Name == category).Count()
                    },
                    CurrentCategory = category,
                    SearchString = null
                }) ;
        }


        //[HttpPost]
        //public IActionResult Catalog(string? Search, string category, int productPage = 1)
        //{
        //    if (Search != null)
        //    {
        //        return View(new ProductsListViewModel
        //        {
        //            Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
        //            .OrderBy(p => p.Id)
        //            .Where(p => (p.Name.ToLower().Contains(Search.ToLower()) || p.Categories.Name.ToLower().Contains(Search.ToLower()) || p.Brands.Name.ToLower().Contains(Search.ToLower())) &&
        //                (category == null || p.Categories.Name == category)),
        //            PagingInfo = new PagingInfo
        //            {
        //                CurrentPage = productPage,
        //                ItemsPerPage = Context.Products.Count(),
        //                TotalItems = category == null ?
        //                    Context.Products.Count() :
        //                    Context.Products.Include(a => a.Categories).Where(a => a.Categories.Name == category).Count()
        //            },
        //            SearchString = Search,
        //            CurrentCategory = category
        //        });
        //    }
        //    else
        //    {
        //        return View(new ProductsListViewModel
        //        {
        //            Products = Context.Products.Include(b => b.Brands).Include(b => b.Categories)
        //            .OrderBy(a => a.Id)
        //            .Where(p => category == null || p.Categories.Name == category)
        //            .Skip((productPage - 1) * PageSize)
        //            .Take(PageSize),
        //            PagingInfo = new PagingInfo
        //            {
        //                CurrentPage = productPage,
        //                ItemsPerPage = PageSize,
        //                TotalItems = category == null ?
        //        Context.Products.Count() :
        //        Context.Products.Include(a => a.Categories).Where(a => a.Categories.Name == category).Count()
        //            },
        //            CurrentCategory = category,
        //            SearchString = null
        //        });
        //    }
        //}
    }
}