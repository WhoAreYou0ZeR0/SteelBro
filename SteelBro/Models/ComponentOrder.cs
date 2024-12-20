using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class ComponentOrder
    {
        public int OrderId { get; set; }

        public int ComponentId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Component Component { get; set; }
    }
}
