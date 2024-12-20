using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ServiceName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(30)]
        public string? Comment { get; set; } // Может быть NULL

        // Navigation property
        public ICollection<Order>? Orders { get; set; } // Может быть NULL
    }
}