namespace SteelBro.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; } // Номер заказа
        public string ServiceName { get; set; } // Название услуги
        public string StatusName { get; set; } // Статус заказа
        public DateTime OrderDate { get; set; } // Дата начала
        public DateTime? ExDate { get; set; } // Дата окончания
        public decimal Price { get; set; } // Цена
        public string? Comment { get; set; } // Добавляем свойство для комментария
    }
}
