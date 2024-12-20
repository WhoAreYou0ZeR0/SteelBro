using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }
}
