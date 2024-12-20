using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(20)]
        public string StatusName { get; set; }

        // Navigation property
        public ICollection<Order> Orders { get; set; }
    }
}
