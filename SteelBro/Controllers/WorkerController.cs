using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Models;

namespace SteelBro.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Technician, Financier")] // Доступ для всех, кроме клиента
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkerController(ApplicationDbContext context)
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
                .Include(u => u.Worker) // Подключаем данные работника
                .ThenInclude(w => w.Post) // Подключаем данные должности
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || user.Worker == null)
            {
                return NotFound("Работник не найден");
            }

            // Создаем модель для представления
            var model = new WorkerProfileViewModel
            {
                FullName = $"{user.Worker.LastName} {user.Worker.FirstName} {user.Worker.MiddleName}",
                Username = user.Username,
                PhoneNumber = user.Worker.PhoneNumber.ToString(),
                Email = user.Worker.Email,
                PostName = user.Worker.Post?.PostName // Должность работника
            };

            return View(model);
        }
    }
}
