namespace Cloth.Models
{
    public class Commentaries
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public Products Product { get; set; }
    }
}
