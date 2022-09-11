using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Cloth.Models
{
    public class Order
    {
        [BindNever]
        public int Id { get; set; }
        [BindNever]
        public ICollection<CartLine>? Lines { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите Фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите адрес")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Введите индекс")]
        public string Index { get; set; }

        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public bool UsingPromocode { get; set; }
        public int PromocodePercent { get; set; }
        public string? Status { get; set; }
        public string? Refund { get; set; }

    }
}
