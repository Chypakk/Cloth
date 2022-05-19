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
        private int PageSize = 8;
        private CookieOptions opt = new CookieOptions();

        public DataContext Context { get; set; }
        public ProductsListViewModel productsListViewModel { get; set; }
        public HomeController(ILogger<HomeController> logger, DataContext ctx)
        {
            _logger = logger;
            Context = ctx;
        }

        public IActionResult CompanyInfo() => View();
        public IActionResult ContactInfo() => View();
        public IActionResult ChangeAndRefund() => View();
        public IActionResult FAQ() => View();
        public IActionResult Delivery() => View();
        public IActionResult Index() => View(Context.Products.OrderByDescending(a => a.Id).Take(PageSize).ToList());
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });   
    }
}