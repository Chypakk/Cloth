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

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите адрес")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        //public int ClientId { get; set; }
        //public int CardId { get; set; }
        //public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public int Quantity { get; set; }
        //public double Price { get; set; }
        //public string? Promocode { get; set; }
        //public DateTime Created { get; set; }
        //public string Status { get; set; }

        //public ClientsData ClientsData { get; set; }
        //public CreditCard CreditCard { get; set; }
        //public List<Products> Products { get; set; }
    }
}
