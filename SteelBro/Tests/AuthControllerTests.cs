using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SteelBro.Controllers;
using SteelBro.Models;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace SteelBro.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly ApplicationDbContext _context;

        public AuthControllerTests()
        {
            // Настройка контекста базы данных в памяти для тестов
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Уникальное имя для каждого теста
                .Options;

            _context = new ApplicationDbContext(options);

            // Очистка базы данных перед каждым тестом
            _context.Database.EnsureDeleted();

            // Инициализация тестовых данных
            _context.Roles.Add(new Role { RoleId = 1, RoleName = "Client" });
            _context.SaveChanges();
        }
        [Fact]
        public async Task Index_InvalidCredentials_ReturnsViewWithModelError()
        {
            // Arrange
            var controller = new AuthController(_context);

            // Очистка базы данных перед каждым тестом
            _context.Database.EnsureDeleted();

            // Инициализация тестовых данных с уникальными ключами
            _context.Roles.Add(new Role { RoleId = 2, RoleName = "Client" });
            _context.SaveChanges();

            // Act
            var result = await controller.Index("testuser", "wrongpassword");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public async Task RegisterStep2_ValidData_CreatesUserAndClient()
        {
            // Arrange
            var controller = new AuthController(_context);

            // Очистка базы данных перед каждым тестом
            _context.Database.EnsureDeleted();

            // Инициализация тестовых данных с уникальными ключами
            _context.Roles.Add(new Role { RoleId = 3, RoleName = "Client" });
            _context.SaveChanges();

            // Установка данных в сессии
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Session = new MockHttpSession();
            controller.HttpContext.Session.SetString("Username", "testuser");
            controller.HttpContext.Session.SetString("Password", "testpassword");

            // Act
            var result = await controller.RegisterStep2("John", "Doe", "Smith", "1234567890", "john@example.com");

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == user.UserId);

            Assert.NotNull(user);
            Assert.NotNull(client);
        }

        private (string passwordHash, string salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = Convert.ToBase64String(hmac.Key);
                var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return (passwordHash, salt);
            }
        }
    }

    // Вспомогательный класс для имитации сессии
    public class MockHttpSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear()
        {
            _sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _sessionStorage.TryGetValue(key, out value);
        }
    }
}