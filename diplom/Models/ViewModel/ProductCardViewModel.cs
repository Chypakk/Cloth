namespace Cloth.Models.ViewModel
{
    public class ProductCardViewModel
    {
        public Products Products { get; set; }
        public IEnumerable<Picture> Picture { get; set; }
        public Options Options { get; set; }
        public IEnumerable<Commentaries> Commentaries { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
