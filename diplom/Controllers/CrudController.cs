using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cloth.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CrudCommand<T> where T : class
    {
        private DataContext context;

        public CrudCommand(DataContext dataContext)
        {
            context = dataContext;
        }

        public async Task Create(T model)
        {
            context.Set<T>().Add(model);
            context.SaveChanges();
        }
        public async Task Update(T model)
        {
            context.Set<T>().Update(model);
            context.SaveChanges();
        }
        public async Task Delete(int id)
        {
            T item = context.Set<T>().Find(id);
            context.Set<T>().Remove(item);
            context.SaveChanges();
        }
        public async Task Delete(Guid id)
        {
            T item = context.Set<T>().Find(id);
            context.Set<T>().Remove(item);
            context.SaveChanges();
        }
    }

    [Authorize(Roles = "Admin")]
    public class CrudController : Controller
    {
        private DataContext Context { get; set; }

        public CrudController(DataContext ctx) { Context = ctx; }

        public IActionResult CrudView() => View();
        private byte[] ConvertToBytes(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        //бренды
        public IActionResult BrandsView() => View(Context.Brands.OrderBy(a => a.Id));
        public IActionResult BrandsUpdate(int Id) => View(Context.Brands.FirstOrDefault(a => a.Id == Id));
        public IActionResult BrandsAdd() => View();

        [HttpPost]
        public async Task<IActionResult> BrandsAdd(Brand brand)
        {
            CrudCommand<Brand> crud = new CrudCommand<Brand>(Context);
            await crud.Create(brand);
            return RedirectToAction("BrandsView", Context.Brands);
        }
        [HttpPost]
        public async Task<IActionResult> BrandsUpdate(Brand brand)
        {
            CrudCommand<Brand> crud = new CrudCommand<Brand>(Context);
            await crud.Update(brand);
            return RedirectToAction("BrandsView");
        }
        public async Task<IActionResult> BrandsDelete(int Id)
        {
            CrudCommand<Brand> crud = new CrudCommand<Brand>(Context);
            await crud.Delete(Id);
            return RedirectToAction("BrandsView");
        }

        //категории
        public IActionResult CategoriesView() => View(Context.Categories.OrderBy(a => a.Id));
        public IActionResult CategoriesUpdate(int Id) => View(Context.Categories.FirstOrDefault(a => a.Id == Id));
        public IActionResult CategoriesAdd() => View();
        [HttpPost]
        public async Task<IActionResult> CategoriesAdd(Category category)
        {
            CrudCommand<Category> crud = new CrudCommand<Category>(Context);
            await crud.Create(category);
            return RedirectToActionPermanent("CategoriesView", Context.Categories);
        }
        [HttpPost]
        public async Task<IActionResult> CategoriesUpdate(Category category)
        {
            CrudCommand<Category> crud = new CrudCommand<Category>(Context);
            await crud.Update(category);
            return RedirectToAction("CategoriesView");
        }
        public async Task<IActionResult> CategoriesDelete(int Id)
        {
            CrudCommand<Category> crud = new CrudCommand<Category>(Context);
            await crud.Delete(Id);
            return RedirectToAction("CategoriesView");
        }

        //продукты
        public IActionResult ProductView() => View(Context.Products.OrderBy(a => a.Id).Include(a => a.Brands).Include(a => a.Categories).Include(a => a.Options));
        public IActionResult ProductUpdate(int Id)
        {
            var result = new CRUDVIewModel
            {
                Products = Context.Products.Where(p => p.Id == Id),
                Brands = Context.Brands,
                Categories = Context.Categories,
                Options = Context.Options
            };
            return View(result);
        }
        public IActionResult ProductAdd() => View(new CRUDVIewModel
        {
            Brands = Context.Brands,
            Options = Context.Options,
            Categories = Context.Categories,
        });

        [HttpPost]
        public async Task<IActionResult> ProductAdd(CRUDVIewModel model, IFormFile file)
        {
            byte[] ImageData = ConvertToBytes(file);
            if (model.CategoryId != 0 && model.BrandId != 0
                && model.OptionsId != 0 && model.Price != 0 && model.ProductName != null)
            {
                Products products = new Products
                {
                    BrandId = model.BrandId,
                    CategoryId = model.CategoryId,
                    OptionsId = model.OptionsId,
                    Price = model.Price,
                    Name = model.ProductName
                };
                products.ProductImage = ImageData;

                CrudCommand<Products> crud = new CrudCommand<Products>(Context);
                await crud.Create(products);
                return RedirectToAction("ProductView");
            }
            else
            {
                return RedirectToAction("ProductAdd");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(CRUDVIewModel model, IFormFile file)
        {
            if (file != null)
            {
                byte[] ImageData = ConvertToBytes(file);
                Products product = Context.Products
                    .FirstOrDefault(a => a.Id == model.ProductId);
                product.ProductImage = ImageData;

                CrudCommand<Products> crud = new CrudCommand<Products>(Context);
                await crud.Update(product);
                return RedirectToAction("ProductView");
            }
            else
            {
                return RedirectToAction("ProductUpdate");
            }
        }
        public async Task<IActionResult> ProductDelete(int Id)
        {
            CrudCommand<Products> crud = new CrudCommand<Products>(Context);
            await crud.Delete(Id);
            return RedirectToAction("ProductView");
        }


        //доп инфа
        public IActionResult OptionsView() => View(Context.Options.OrderBy(a => a.Id));
        public IActionResult OptionsUpdate(int Id) => View(Context.Options.FirstOrDefault(a => a.Id == Id));
        public IActionResult OptionsAdd() => View();
        [HttpPost]
        public async Task<IActionResult> OptionsAdd(Options options)
        {
            CrudCommand<Options> crud = new CrudCommand<Options>(Context);
            await crud.Create(options);
            return RedirectToAction("OptionsView");
        }
        [HttpPost]
        public async Task<IActionResult> OptionsUpdate(Options options)
        {
            CrudCommand<Options> crud = new CrudCommand<Options>(Context);
            await crud.Update(options);
            return RedirectToAction("OptionsView");
        }
        public async Task<IActionResult> OptionsDelete(int Id)
        {
            CrudCommand<Options> crud = new CrudCommand<Options>(Context);
            await crud.Delete(Id);
            return RedirectToAction("OptionsView");
        }

        //комментарии
        [HttpPost]
        public async Task<IActionResult> Commentaries(Commentaries commentaries, int ComId, string ComName)
        {
            commentaries.CreatedDate = DateTime.Now.ToLocalTime().ToUniversalTime();
            CrudCommand<Commentaries> crud = new CrudCommand<Commentaries>(Context);
            await crud.Create(commentaries);
            return RedirectToAction("ProductCard", "Catalog", new { ComId, ComName });
        }

        //остатки
        public IActionResult RemainsView() => View(Context.Remains.Include(a => a.Products));
        public IActionResult RemainsUpdate(Guid Id) => View(Context.Remains.Include(a => a.Products).FirstOrDefault(a => a.Id == Id));
        public IActionResult RemainsAdd()
        {
            var result = new CRUDVIewModel
            {
                Products = Context.Products
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> RemainsAdd(CRUDVIewModel model)
        {
            if (model.ProductId != null && model.Count != null && model.Size != null)
            {
                Remains remAdd = new Remains
                {
                    ProductId = model.ProductId,
                    Size = model.Size,
                    Count = model.Count

                };
                CrudCommand<Remains> crud = new CrudCommand<Remains>(Context);
                await crud.Create(remAdd);
                return RedirectToAction("RemainsView");
            }
            else
            {
                ModelState.AddModelError("", "Введите данные");
                return RedirectToAction("RemainsAdd");
            }

        }
        [HttpPost]
        public async Task<IActionResult> RemainsUpdate(Remains remains)
        {
            CrudCommand<Remains> crud = new CrudCommand<Remains>(Context);
            await crud.Update(remains);
            return RedirectToAction("RemainsView");
        }
        public async Task<IActionResult> RemainsDelete(Guid Id)
        {
            CrudCommand<Remains> crud = new CrudCommand<Remains>(Context);
            await crud.Delete(Id);
            return RedirectToAction("RemainsView");
        }

        //Промокод
        public IActionResult PromocodeView() => View(Context.Promocodes.OrderBy(a => a.Id));
        public IActionResult PromocodeUpdate(int Id) => View(Context.Promocodes.FirstOrDefault(a => a.Id == Id));
        public IActionResult PromocodeAdd() => View();
        [HttpPost]
        public async Task<IActionResult> PromocodeAdd(Promocode promocode)
        {
            Promocode.PromocodeRemake(promocode);

            CrudCommand<Promocode> crud = new CrudCommand<Promocode>(Context);
            await crud.Create(promocode);
            return RedirectToAction("PromocodeView");
        }
        [HttpPost]
        public async Task<IActionResult> PromocodeUpdate(Promocode promocode)
        {
            CrudCommand<Promocode> crud = new CrudCommand<Promocode>(Context);
            await crud.Update(promocode);
            return RedirectToAction("PromocodeView");
        }
        public async Task<IActionResult> PromocodeDelete(int Id)
        {
            CrudCommand<Promocode> crud = new CrudCommand<Promocode>(Context);
            await crud.Delete(Id);
            return RedirectToAction("PromocodeView");
        }

        //статус заказов
        public IActionResult Orders()
        {
            OrdersViewModel result = new OrdersViewModel
            {
                Order = Context.Orders.Include(a => a.Lines).ToList(),
                CartLines = Context.CartLine.Include(a => a.Product).ToList()
            };
            return View(result);
        }
        [HttpPost]
        public IActionResult OrdersStatus(int Id, string Status)
        {
            var result = Context.Orders.FirstOrDefault(a => a.Id == Id);
            result.Status = Status;
            Context.SaveChanges();
            return RedirectToAction("Orders", new OrdersViewModel
            {
                Order = Context.Orders.Include(a => a.Lines),
                
            });
        }

    }
}
