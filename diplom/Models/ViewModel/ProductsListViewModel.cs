using Cloth.Models;

namespace Cloth.Models.ViewModel
{
    public class ProductsListViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public string Search { get; set; }
        public string CurrentBrand { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
