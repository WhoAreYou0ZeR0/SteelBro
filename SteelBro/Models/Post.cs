using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [Required]
        [MaxLength(20)]
        public string PostName { get; set; }

        // Навигационное свойство
        public ICollection<Worker> Workers { get; set; }
    }
}
