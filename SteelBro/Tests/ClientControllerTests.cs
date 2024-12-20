using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Controllers;
using SteelBro.Models;
using System.Security.Claims;
using Xunit;

namespace SteelBro.Tests.Controllers
{
    public class ClientControllerTests
    {
        private readonly ApplicationDbContext _context;

        public ClientControllerTests()
        {
            // Настройка контекста базы данных в памяти для тестов
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            _context.Users.Add(new User
            {
                UserId = 1,
                Username = "testuser",
                PasswordHash = "hashedpassword", // Заполняем обязательные свойства
                Salt = "salt", // Заполняем обязательные свойства
                RoleId = 1,
                Client = new Client
                {
                    ClientId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    MiddleName = "Michael", 
                    PhoneNumber = 1234567890,
                    Email = "john@example.com"
                }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ValidUser_ReturnsViewWithClientProfile()
        {
            var controller = new ClientController(_context);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testuser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ClientProfileViewModel>(viewResult.Model);
            Assert.Equal("Doe John Michael", model.FullName); // Обновляем ожидаемое значение
            Assert.Equal("testuser", model.Username);
            Assert.Equal("1234567890", model.PhoneNumber);
            Assert.Equal("john@example.com", model.Email);
        }
    }
}