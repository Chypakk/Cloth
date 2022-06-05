using System.ComponentModel.DataAnnotations;

namespace Cloth.Models.ViewModel
{
    public class CRUDVIewModel
    {

        public IEnumerable<Brand>? Brands { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Products>? Products { get; set; }
        public IEnumerable<Options>? Options { get; set; }
        public IEnumerable<Remains>? Remains { get; set; }
        //остатки
        public int ProductId { get; set; }
        public string? Size { get; set; }
        public int Count { get; set; }
        //продукт
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int OptionsId { get; set; }
        public int Price { get; set; }
    }
}
