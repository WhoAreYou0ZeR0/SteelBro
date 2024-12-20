using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Worker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkerId { get; set; }

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

        [MaxLength(60)]
        public string? Email { get; set; } // Может быть NULL

        // Navigation property
        [ForeignKey("Post")]
        [Column("Post")] // Указываем имя столбца в БД
        public int PostId { get; set; }

        public Post Post { get; set; }  // Связь с Post

        // Foreign key
        public int? UserId { get; set; } // Может быть NULL

        // Navigation property
        [ForeignKey("UserId")]
        public User? User { get; set; } // Может быть NULL

        // Navigation property
        public ICollection<Order>? Orders { get; set; } // Может быть NULL
    }
}