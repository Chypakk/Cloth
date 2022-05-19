using System.ComponentModel.DataAnnotations;

namespace Cloth.Models
{
    public class Picture
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
