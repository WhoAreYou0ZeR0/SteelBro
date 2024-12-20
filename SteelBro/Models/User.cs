using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SteelBro.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(255)]
        public string Salt { get; set; }

        // Foreign key
        public int RoleId { get; set; }

        // Navigation properties
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public Client Client { get; set; }
        public Worker Worker { get; set; }
    }
}
