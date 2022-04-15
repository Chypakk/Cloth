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

        public IActionResult BrandsView() => View(Context.Brands.OrderBy(a => a.Id));
        public IActionResult BrandsAdd() => View();
        [HttpPost]
        public IActionResult BrandsAdd(Brand brand)
        {
            Context.Brands.Add(brand);
            Context.SaveChanges();
            return RedirectToAction("BrandsView", Context.Brands);
        }

        public IActionResult CategoriesView() => View(Context.Categories.OrderBy(a => a.Id));
        public IActionResult CategoriesAdd() => View();
        [HttpPost]
        public IActionResult CategoriesAdd(Category category)
        {
            Context.Categories.Add(category);
            Context.SaveChanges();
            return RedirectToActionPermanent("CategoriesView", Context.Categories);
        }

        public IActionResult RemainsView() => View(Context.Remains.OrderBy(a => a.Id));
        public IActionResult RemainsAdd() => View();
        [HttpPost]
        public IActionResult RemainsAdd(Remains remains)
        {
            Context.Remains.Add(remains);
            Context.SaveChanges();
            return RedirectToAction("ReaminsView", Context.Remains);
        }

        //public IActionResult WarehouseView() => View(Context.)
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
            up.Price = product.Price;
            up.BrandId = product.BrandId;
            up.Color = product.Color;
            up.Rating = product.Rating;
            up.Size = product.Size;
            up.ProductImage = ImageData;
            Context.SaveChanges();
            return RedirectToAction("ProductView");
        }

    }
}
