using Cloth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cloth.Controllers
{
    public class RefundController : Controller
    {
        private DataContext Context { get; set; }
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        public RefundController(UserManager<AppUser> userMngr, SignInManager<AppUser> signInMngr, DataContext ctx)
        {
            Context = ctx;
            userManager = userMngr;
            signInManager = signInMngr;
        }

        public IActionResult Refunds() => View(Context.Refunds);
        public IActionResult ProfileRefunds() => View("Refunds",Context.Refunds.Where(a => a.UserName == User.Identity.Name));
        public IActionResult RefundForm() => View();
        [HttpPost]
        public IActionResult RefundForm(int Id, string UserName)
        {

            ViewBag.OrderId = Id;
            return View();

        }
        public IActionResult AdminRefundForm(int Id)
        {
            var Refund = Context.Refunds.FirstOrDefault(a => a.Id == Id);
            return View(Refund);
        }
        [HttpPost]
        public IActionResult AdminRefundForm(Refund refund, string Status)
        {
            var update = Context.Refunds.FirstOrDefault(a => a.Id == refund.Id);
            var order = Context.Orders.FirstOrDefault(a => a.Id == update.OrderId);
            if (Status == "Возврат отклонен")
            {
                order.Status = "Ожидание";
            }
            else
            {
                order.Status = "Возврат";
            }

            order.Refund = Status;
            update.AdminName = User.Identity.Name;
            update.Result = Status;
            update.AdminText = refund.AdminText;
            update.UpdatedDate = DateTime.Now;
            Context.Update(update);
            Context.SaveChanges();
            return RedirectToAction("Refunds", Context.Refunds);
        }

        public async Task<IActionResult> RefundFormSave(Refund refund, int OrderId)
        {
            AppUser upUser = await userManager.FindByNameAsync(User.Identity.Name);
            bool Role = await userManager.IsInRoleAsync(upUser, "Admin");
            var order = Context.Orders.FirstOrDefault(o => o.Id == OrderId);
            order.Refund = "Ожидание итогов возврата";
            refund.Result = "Ожидание ответа администратора";
            refund.CreatedDate = DateTime.Now;
            refund.UserName = User.Identity.Name;
            refund.OrderId = OrderId;
            Context.Add(refund);
            Context.SaveChanges();
            if (Role)
            {
                return RedirectToAction("AdminProfile", "Account");
            }
            else
            {
                return RedirectToAction("Profile", "Account");
            }
        }
    }
}
