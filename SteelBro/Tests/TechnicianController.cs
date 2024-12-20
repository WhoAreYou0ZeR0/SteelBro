using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Controllers;
using SteelBro.Models;
using Xunit;

namespace SteelBro.Tests.Controllers
{
    public class TechnicianControllerTests
    {
        private readonly ApplicationDbContext _context;

        public TechnicianControllerTests()
        {
            // Настройка контекста базы данных в памяти для тестов
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Очистка базы данных перед каждым тестом
            _context.Database.EnsureDeleted();

            // Инициализация тестовых данных
            _context.Components.AddRange(
                new Component { ComponentId = 1, ComponentName = "RAM", Price = 50.00m, Quantity = 10 },
                new Component { ComponentId = 2, ComponentName = "CPU", Price = 150.00m, Quantity = 5 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetComponents_ReturnsListOfComponents()
        {
            var controller = new TechnicianController(_context);

            var result = await controller.GetComponents();

            var jsonResult = Assert.IsType<JsonResult>(result);
            var components = Assert.IsAssignableFrom<IEnumerable<object>>(jsonResult.Value);

            var componentsList = components.ToList();

            // Проверяем, что список содержит ожидаемые данные
            Assert.Equal(2, componentsList.Count);

            var firstComponent = componentsList[0];
            var firstComponentProps = firstComponent.GetType().GetProperties();

            Assert.Equal(1, firstComponentProps.First(p => p.Name == "ComponentId").GetValue(firstComponent));
            Assert.Equal("RAM", firstComponentProps.First(p => p.Name == "ComponentName").GetValue(firstComponent));
            Assert.Equal(50.00m, firstComponentProps.First(p => p.Name == "Price").GetValue(firstComponent));
            Assert.Equal(10, firstComponentProps.First(p => p.Name == "Quantity").GetValue(firstComponent));
        }
    }
}