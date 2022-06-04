namespace Cloth.Models
{
    public class Refund
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string? AdminName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserText { get; set; }
        public string? AdminText { get; set; }
        public string Result { get; set; }
    }
}
