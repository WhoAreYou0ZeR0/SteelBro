namespace SteelBro.Models
{
    public class ClientProfileViewModel
    {
        public string FullName { get; set; } // ФИО
        public string Username { get; set; } // Имя пользователя
        public string PhoneNumber { get; set; } // Номер телефона
        public string Email { get; set; } // Email
        public List<OrderViewModel> Orders { get; set; } // Список заказов
    }
}
