using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SteelBro.Models
{
    public class AddOrderViewModel
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Выберите услугу")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Выберите работника")]
        public int WorkerId { get; set; }

        public List<SelectListItem> Services { get; set; }
        public List<SelectListItem> Workers { get; set; }
        public List<Component> Components { get; set; }

        public List<SelectedComponentViewModel> SelectedComponents { get; set; } = new();
    }

    public class SelectedComponentViewModel
    {
        public int ComponentId { get; set; }
        public int Quantity { get; set; }
    }
}
