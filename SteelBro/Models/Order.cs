using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? ExDate { get; set; } // Может быть NULL

        [MaxLength(255)] // Установите максимальную длину, если нужно
        public string? Comment { get; set; } // Добавляем поле для комментария

        // Navigation properties
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [ForeignKey("WorkerId")]
        public Worker Worker { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        // Navigation property
        public ICollection<ComponentOrder>? ComponentOrders { get; set; } // Может быть NULL
    }
}