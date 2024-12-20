using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? MiddleName { get; set; } // Может быть NULL

        [Required]
        public long PhoneNumber { get; set; }

        [MaxLength(40)]
        public string? Email { get; set; } // Может быть NULL

        // Foreign key
        public int UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Navigation property
        public ICollection<Order>? Orders { get; set; } // Может быть NULL
    }
}