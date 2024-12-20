using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Models; 
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SteelBro.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Страница входа
        public IActionResult Index()
        {
            return View();
        }

        // Обработка входа
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            // Проверяем пользователя в базе данных
            var user = await _context.Users
                .Include(u => u.Role) // Подключаем роль пользователя
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль");
                return View();
            }

            // Создаем ClaimsIdentity
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName) // Роль пользователя
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Вход пользователя
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Перенаправление в зависимости от роли
            switch (user.Role.RoleName)
            {
                case "Administrator":
                    return RedirectToAction("Index", "Admin");
                case "Manager":
                    return RedirectToAction("Index", "Manager");
                case "Technician":
                    return RedirectToAction("Index", "Technician");
                case "Financier":
                    return RedirectToAction("Index", "Financier");
                case "Client":
                    return RedirectToAction("Index", "Client");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        // Страница регистрации
        public IActionResult RegisterStep1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterStep1(string username, string password)
        {
            // Проверяем, существует ли пользователь с таким именем
            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError("", "Пользователь с таким именем уже существует");
                return View("RegisterStep1");
            }

            // Сохраняем данные в сессии
            HttpContext.Session.SetString("Username", username);
            HttpContext.Session.SetString("Password", password);

            // Перенаправляем на второй шаг
            return RedirectToAction("RegisterStep2");
        }

        // Страница регистрации
        public IActionResult RegisterStep2()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStep2(string first_name, string last_name, string middle_name, string phone, string email)
        {
            // Извлекаем данные из сессии
            var username = HttpContext.Session.GetString("Username");
            var password = HttpContext.Session.GetString("Password");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("RegisterStep1"); // Если данные отсутствуют, перенаправляем на первый шаг
            }

            // Хешируем пароль
            var (passwordHash, salt) = HashPassword(password);

            // Получаем роль Client
            var clientRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Client");
            if (clientRole == null)
            {
                ModelState.AddModelError("", "Роль 'Client' не найдена");
                return View("RegisterStep2");
            }

            // Создаем нового пользователя
            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt,
                RoleId = clientRole.RoleId
            };

            // Добавляем пользователя в базу данных
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Создаем клиента
            var newClient = new Client
            {
                FirstName = first_name,
                LastName = last_name,
                MiddleName = middle_name,
                PhoneNumber = long.Parse(phone),
                Email = email,
                UserId = newUser.UserId
            };

            // Добавляем клиента в базу данных
            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            // Очищаем сессию
            HttpContext.Session.Clear();

            // Перенаправляем на страницу входа
            return RedirectToAction("Index");
        }

        // Выход пользователя
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Метод для хеширования пароля
        private (string passwordHash, string salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return (passwordHash, salt);
            }
        }

        // Метод для проверки пароля
        private bool VerifyPassword(string password, string passwordHash, string salt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(salt)))
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return computedHash == passwordHash;
            }
        }
    }
}