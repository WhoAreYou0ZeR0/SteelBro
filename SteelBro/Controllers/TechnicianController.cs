using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Models;

namespace SteelBro.Controllers
{
    [Authorize(Roles = "Technician")]
    public class TechnicianController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechnicianController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Components() { return View(); }
        public IActionResult AddComponent() { return View(); }

        // Отображение страницы с заказами
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Client) // Подключаем данные клиента
                .Include(o => o.Service) // Подключаем данные услуги
                .Include(o => o.Status) // Подключаем данные статуса
                .ToListAsync();

            return View(orders);
        }

        // Получение списка статусов для фильтра
        [HttpGet]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _context.Status.ToListAsync();
            return Json(statuses);
        }

        // Получение списка заказов с фильтрацией
        [HttpGet]
        public async Task<IActionResult> GetOrders(string clientName = null, string statusName = null)
        {
            var orders = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.Status)
                .AsQueryable();

            if (!string.IsNullOrEmpty(clientName))
            {
                orders = orders.Where(o => o.Client.FirstName.Contains(clientName) || o.Client.LastName.Contains(clientName));
            }

            if (!string.IsNullOrEmpty(statusName) && statusName != "all")
            {
                orders = orders.Where(o => o.Status.StatusName == statusName);
            }

            var result = await orders.Select(o => new
            {
                o.OrderId,
                ClientName = $"{o.Client.FirstName} {o.Client.LastName}",
                o.Service.ServiceName,
                o.Status.StatusName,
                o.OrderDate,
                o.ExDate
            }).ToListAsync();

            return Json(result);
        }
        public async Task<IActionResult> ViewOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                    .ThenInclude(c => c.User) // Подключаем данные пользователя клиента
                .Include(o => o.Service)
                .Include(o => o.Status)
                .Include(o => o.ComponentOrders) // Подключаем данные компонентов заказа
                    .ThenInclude(co => co.Component) // Подключаем данные компонентов
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Получаем список всех статусов
            var statuses = await _context.Status.ToListAsync();
            ViewBag.Statuses = statuses;

            return View(order);
        }

        // Изменение статуса заказа
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateStatusRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.NewStatus))
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var order = await _context.Orders
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId);

            if (order == null)
            {
                return Json(new { success = false, message = "Заказ не найден" });
            }

            var status = await _context.Status.FirstOrDefaultAsync(s => s.StatusName == request.NewStatus);
            if (status == null)
            {
                return Json(new { success = false, message = "Указанный статус не существует" });
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Статус заказа обновлён", updatedStatus = status.StatusName });
        }

        public class UpdateStatusRequest
        {
            public int OrderId { get; set; }
            public string NewStatus { get; set; }
        }

        // Получение списка компонентов
        [HttpGet]
        public async Task<IActionResult> GetComponents(string componentName = null)
        {
            var components = _context.Components.AsQueryable();

            if (!string.IsNullOrEmpty(componentName))
            {
                components = components.Where(c => c.ComponentName.Contains(componentName));
            }

            var result = await components.Select(c => new
            {
                c.ComponentId,
                c.ComponentName,
                c.Quantity,
                c.Price,
                c.Specifications
            }).ToListAsync();

            return Json(result);
        }

        // Просмотр компонента
        public async Task<IActionResult> ViewComponent(int id)
        {
            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.ComponentId == id);

            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }

        // Обновление количества компонента
        [HttpPost]
        public async Task<IActionResult> UpdateComponentQuantity([FromBody] UpdateComponentQuantityRequest request)
        {
            if (request == null || request.ComponentId <= 0 || request.NewQuantity < 0)
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.ComponentId == request.ComponentId);

            if (component == null)
            {
                return Json(new { success = false, message = "Компонент не найден" });
            }

            component.Quantity = request.NewQuantity;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Количество компонента обновлено", updatedQuantity = component.Quantity });
        }

        // Обновление цены компонента
        [HttpPost]
        public async Task<IActionResult> UpdateComponentPrice([FromBody] UpdateComponentPriceRequest request)
        {
            if (request == null || request.ComponentId <= 0 || request.NewPrice < 0)
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.ComponentId == request.ComponentId);

            if (component == null)
            {
                return Json(new { success = false, message = "Компонент не найден" });
            }

            component.Price = request.NewPrice;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Цена компонента обновлена", updatedPrice = component.Price });
        }

        public class UpdateComponentQuantityRequest
        {
            public int ComponentId { get; set; }
            public int NewQuantity { get; set; }
        }

        public class UpdateComponentPriceRequest
        {
            public int ComponentId { get; set; }
            public decimal NewPrice { get; set; }
        }
        // Добавление нового компонента
        [HttpPost]
        public async Task<IActionResult> AddComponent([FromBody] AddComponentRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ComponentName) || request.Quantity < 0 || request.Price < 0)
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var newComponent = new Component
            {
                ComponentName = request.ComponentName,
                Specifications = request.Specifications,
                Quantity = request.Quantity,
                Price = request.Price
            };

            _context.Components.Add(newComponent);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Компонент успешно добавлен", componentId = newComponent.ComponentId });
        }

        // Удаление компонента
        [HttpPost]
        public async Task<IActionResult> DeleteComponent([FromBody] DeleteComponentRequest request)
        {
            if (request == null || request.ComponentId <= 0)
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.ComponentId == request.ComponentId);

            if (component == null)
            {
                return Json(new { success = false, message = "Компонент не найден" });
            }

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Компонент успешно удалён" });
        }

        public class AddComponentRequest
        {
            public string ComponentName { get; set; }
            public string Specifications { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public class DeleteComponentRequest
        {
            public int ComponentId { get; set; }
        }

    }
}
