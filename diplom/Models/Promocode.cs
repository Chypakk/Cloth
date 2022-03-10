namespace Cloth.Models
{
    public class Promocode
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Percent { get; set; }
        public string Code { get; set; }
    }
}
