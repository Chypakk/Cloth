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
        private int PageSize = 4;
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

        


        
    }
}