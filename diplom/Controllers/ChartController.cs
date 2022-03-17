using Cloth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Cloth.Controllers
{
    public class ChartController : Controller
    {
        private AddDbConnect context;

        public ChartController(AddDbConnect ctx)
        {
            context = ctx;
        }
        public IActionResult Chart() => View(context.CartLine.Include(q=>q.Product));
    }
}
