using Cloth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloth.Controllers
{
    public class CrudController : Controller
    {
        private AddDbConnect Context { get; set; }

        private byte[] ConvertToBytes(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public CrudController(AddDbConnect ctx) { Context = ctx; }

        public IActionResult CrudView() => View();

        //бренды
        public IActionResult BrandsView() => View(Context.Brands.OrderBy(a => a.Id));
        public IActionResult BrandsUpdate(int Id) => View(Context.Brands.FirstOrDefault(a => a.Id == Id));
        public IActionResult BrandsAdd() => View();
        [HttpPost]
        public IActionResult BrandsAdd(Brand brand)
        {
            Context.Brands.Add(brand);
            Context.SaveChanges();
            return RedirectToAction("BrandsView", Context.Brands);
        }
        [HttpPost]
        public IActionResult BrandsUpdate(Brand brand)
        {
            Brand br = Context.Brands.FirstOrDefault(a => a.Id == brand.Id);
            br.Id = brand.Id;
            br.Name = brand.Name;
            Context.SaveChanges();
            return RedirectToAction("BrandsView");
        }
        public IActionResult BrandsDelete(int Id)
        {
            Brand DelBrand = Context.Brands.Where(a => a.Id == Id).FirstOrDefault();
            Context.Brands.Remove(DelBrand);
            Context.SaveChanges();
            return RedirectToAction("BrandsView");
        }

        //категории
        public IActionResult CategoriesView() => View(Context.Categories.OrderBy(a => a.Id));
        public IActionResult CategoriesUpdate(int Id) => View(Context.Categories.FirstOrDefault(a => a.Id == Id));
        public IActionResult CategoriesAdd() => View();
        [HttpPost]
        public IActionResult CategoriesAdd(Category category)
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
            return RedirectToActionPermanent("CategoriesView", Context.Categories);
        }
        [HttpPost]
        public IActionResult CategoriesUpdate(Category category)
        {
            Category ct = Context.Categories.Where(a => a.Id == category.Id).FirstOrDefault();
            ct.Id = category.Id;
            ct.Name = category.Name;
            Context.SaveChanges();
            return RedirectToAction("CategoriesView");
        }
        public IActionResult CategoriesDelete(int Id)
        {
            Category DelCtg = Context.Categories.FirstOrDefault(a => a.Id == Id);
            Context.Categories.Remove(DelCtg);
            Context.SaveChanges();
            return RedirectToAction("CategoriesView");
        }

        


        //public IActionResult WarehouseView() => View(Context.)

        //продукты
        public IActionResult ProductView() => View(Context.Products.OrderBy(a => a.Id));
        public IActionResult ProductUpdate(int Id) => View(Context.Products.Where(p => p.Id == Id).FirstOrDefault());
        public IActionResult ProductAdd() => View();
        [HttpPost]
        public IActionResult ProductAdd(Products products, IFormFile file)
        {
            byte[] ImageData = ConvertToBytes(file);
            products.ProductImage = ImageData;

            Context.Products.Add(products);
            Context.SaveChanges();
            return RedirectToAction("ProductView");
        }
        [HttpPost]
        public IActionResult ProductUpdate(Products product, IFormFile file)
        {
            byte[] ImageData = ConvertToBytes(file);

            Products up = Context.Products.FirstOrDefault(Product => Product.Id == product.Id);
            up.Id = product.Id;
            up.Name = product.Name;
            up.CategoryId = product.CategoryId;
            up.OptionsId = product.OptionsId;
            up.Price = product.Price;
            up.BrandId = product.BrandId;
            //up.Rating = product.Rating;
            //up.Size = product.Size;
            up.ProductImage = ImageData;
            Context.SaveChanges();
            return RedirectToAction("ProductView");
        }
        public IActionResult ProductDelete(int Id)
        {
            Products DelProduct = Context.Products.Where(a => a.Id == Id).FirstOrDefault();
            Context.Products.Remove(DelProduct);
            Context.SaveChanges();
            return RedirectToAction("ProductView");
        }


        //доп инфа
        public IActionResult OptionsView() => View(Context.Options.OrderBy(a => a.Id));
        public IActionResult OptionsUpdate(int Id) => View(Context.Options.FirstOrDefault(a => a.Id == Id));
        public IActionResult OptionsAdd() => View();
        [HttpPost]
        public IActionResult OptionsAdd(Options options)
        {
            Context.Options.Add(options);
            Context.SaveChanges();
            return RedirectToAction("OptionsView");
        }
        [HttpPost]
        public IActionResult OptionsUpdate(Options options)
        {
            Options op = Context.Options.FirstOrDefault(o => o.Id == options.Id);
            op.Id = options.Id;
            op.Material = options.Material;
            op.CareNote = options.CareNote;
            op.Description = options.Description;
            op.Design = options.Design;
            Context.SaveChanges();
            return RedirectToAction("OptionsView");
        }
        public IActionResult OptionsDelete(int Id)
        {
            Options DelOpt = Context.Options.Where(a => a.Id == Id).FirstOrDefault();
            Context.Options.Remove(DelOpt);
            Context.SaveChanges();
            return RedirectToAction("OptionsView");
        }


        //комментарии
        [HttpPost]
        public IActionResult Commentaries(Commentaries commentaries, int ComId, string ComName)
        {
            commentaries.CreatedDate = DateTime.Now;
            Context.Commentaries.Add(commentaries);
            Context.SaveChanges();
            return RedirectToAction("ProductCard", "Catalog", new {ComId, ComName });
        }

        //остатки
        public IActionResult RemainsView() => View(Context.Remains.OrderBy(a => a.Id));
        public IActionResult RemainsUpdate(Guid Id) => View(Context.Remains.FirstOrDefault(a => a.Id == Id));
        public IActionResult RemainsAdd() => View();
        [HttpPost]
        public IActionResult RemainsAdd(Remains remains)
        {
            Context.Remains.Add(remains);
            Context.SaveChanges();
            return RedirectToAction("RemainsView");
        }
        [HttpPost]
        public IActionResult RemainsUpdate(Remains remains)
        {
            Remains r = Context.Remains.Where(a => a.Id == remains.Id).FirstOrDefault();
            r.Id = remains.Id;
            r.ProductId = remains.ProductId;
            r.Size = remains.Size;
            r.Count = remains.Count;
            Context.SaveChanges();
            return RedirectToAction("RemainsView");
        }
        public IActionResult RemainsDelete(Guid Id)
        {
            Remains DelRem = Context.Remains.FirstOrDefault(a => a.Id == Id);
            Context.Remains.Remove(DelRem);
            Context.SaveChanges();
            return RedirectToAction("RemainsView");
        }
    }
}
