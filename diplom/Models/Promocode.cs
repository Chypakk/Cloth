using System.Text.RegularExpressions;

namespace Cloth.Models
{
    public class Promocode
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Percent { get; set; }
        public string Code { get; set; }

        public static void PromocodeRemake(Promocode promocode)
        {
            promocode.Code = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            promocode.StartDate = promocode.StartDate.ToLocalTime().ToUniversalTime();
            promocode.EndDate = promocode.EndDate.ToLocalTime().ToUniversalTime();
        }
    }
}
