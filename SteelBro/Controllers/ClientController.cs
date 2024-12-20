using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Models;

namespace SteelBro.Controllers
{
    [Authorize(Roles = "Client")] // Доступ только для клиентов
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Получаем текущего авторизованного пользователя
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return RedirectToAction("Index", "Auth"); // Если пользователь не авторизован, перенаправляем на страницу входа
            }

            // Находим пользователя в базе данных
            var user = await _context.Users
                .Include(u => u.Client) // Подключаем данные клиента
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || user.Client == null)
            {
                return NotFound("Клиент не найден");
            }

            // Получаем заказы клиента
            var orders = await _context.Orders
                .Include(o => o.Service) // Подключаем данные услуги
                .Include(o => o.Status) // Подключаем данные статуса
                .Where(o => o.ClientId == user.Client.ClientId)
                .Select(o => new OrderViewModel
                {
                    OrderId = o.OrderId,
                    ServiceName = o.Service.ServiceName,
                    StatusName = o.Status.StatusName,
                    OrderDate = o.OrderDate,
                    ExDate = o.ExDate,
                    Price = o.Service.Price,
                    Comment = o.Comment // Добавляем комментарий
                })
                .ToListAsync();

            // Создаем модель для представления
            var model = new ClientProfileViewModel
            {
                FullName = $"{user.Client.LastName} {user.Client.FirstName} {user.Client.MiddleName}",
                Username = user.Username,
                PhoneNumber = user.Client.PhoneNumber.ToString(),
                Email = user.Client.Email,
                Orders = orders
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Order()
        {
            // Загружаем список услуг из базы данных
            var services = await _context.Services.ToListAsync();
            ViewBag.Services = services;

            // Загружаем список работников из базы данных
            var workers = await _context.Workers.ToListAsync();
            ViewBag.Workers = workers;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Order(int ServiceId, string Description)
        {
            if (ServiceId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Заполните все обязательные поля");
                return View();
            }

            // Получаем текущего авторизованного пользователя
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return RedirectToAction("Index", "Auth"); // Если пользователь не авторизован, перенаправляем на страницу входа
            }

            // Находим пользователя в базе данных
            var user = await _context.Users
                .Include(u => u.Client) // Подключаем данные клиента
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || user.Client == null)
            {
                return NotFound("Клиент не найден");
            }

            // Получаем статус "Новый"
            var defaultStatus = await _context.Status.FirstOrDefaultAsync(s => s.StatusName == "Новый заказ");
            if (defaultStatus == null)
            {
                ModelState.AddModelError(string.Empty, "Статус по умолчанию не найден");
                return View();
            }

            // Получаем первого доступного работника (или выберите подходящего работника)
            var worker = await _context.Workers.FirstOrDefaultAsync();
            if (worker == null)
            {
                ModelState.AddModelError(string.Empty, "Работник не найден");
                return View();
            }

            // Создаем новый заказ
            var newOrder = new Order
            {
                ClientId = user.Client.ClientId,
                ServiceId = ServiceId,
                StatusId = defaultStatus.StatusId,
                WorkerId = worker.WorkerId, // Указываем WorkerId
                OrderDate = DateTime.Now,
                ExDate = null, // Дата окончания пока неизвестна
                Comment = Description // Добавляем описание проблемы
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Перенаправляем на страницу профиля клиента
        }
    }
}
