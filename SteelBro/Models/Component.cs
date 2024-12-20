using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Component
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComponentId { get; set; }


        [Required(ErrorMessage = "Поле Название комплектующего обязательно для заполнения.")]
        [MaxLength(70, ErrorMessage = "Название комплектующего должно быть менее 70 символов.")]
        public string ComponentName { get; set; }

        [Required(ErrorMessage = "Поле Цена обязательно для заполнения.")]
        [Range(0, double.MaxValue, ErrorMessage = "Цена должна быть больше или равна 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Поле Количество обязательно для заполнения.")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0.")]
        public int Quantity { get; set; }

        [MaxLength(100, ErrorMessage = "Технические характеристики должны быть менее 100 символов.")]
        public string? Specifications { get; set; } // Может быть NULL

        // Navigation property
        public ICollection<ComponentOrder>? ComponentOrders { get; set; } // Может быть NULL
    }
}