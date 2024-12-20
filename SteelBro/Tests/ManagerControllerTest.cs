using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Controllers;
using SteelBro.Models;
using Xunit;

namespace SteelBro.Tests.Controllers
{
    public class ManagerControllerTests
    {
        private readonly ApplicationDbContext _context;

        public ManagerControllerTests()
        {
            // Настройка контекста базы данных в памяти для тестов
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            _context.Status.Add(new Status { StatusId = 1, StatusName = "Новый заказ" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetOrders_ReturnsListOfOrders()
        {
            var controller = new ManagerController(_context);

            _context.Orders.Add(new Order
            {
                OrderId = 1,
                ClientId = 1,
                ServiceId = 1,
                StatusId = 1,
                WorkerId = 1,
                OrderDate = DateTime.Now,
                Client = new Client
                {
                    ClientId = 1,
                    FirstName = "John",
                    LastName = "Doe"
                },
                Service = new Service { ServiceName = "Test Service" },
                Status = new Status { StatusName = "New" }
            });
            _context.SaveChanges();

            var result = await controller.GetOrders();

            var jsonResult = Assert.IsType<JsonResult>(result);
            var orders = Assert.IsAssignableFrom<IEnumerable<object>>(jsonResult.Value);

            var ordersList = orders.ToList();

            Assert.Single(ordersList);

            var firstOrder = ordersList[0];
            var firstOrderProps = firstOrder.GetType().GetProperties();

            Assert.Equal(1, firstOrderProps.First(p => p.Name == "OrderId").GetValue(firstOrder));
            Assert.Equal("John Doe", firstOrderProps.First(p => p.Name == "ClientName").GetValue(firstOrder));
            Assert.Equal("Test Service", firstOrderProps.First(p => p.Name == "ServiceName").GetValue(firstOrder));
            Assert.Equal("New", firstOrderProps.First(p => p.Name == "StatusName").GetValue(firstOrder));
            Assert.NotNull(firstOrderProps.First(p => p.Name == "OrderDate").GetValue(firstOrder));
            Assert.Null(firstOrderProps.First(p => p.Name == "ExDate").GetValue(firstOrder));
        }

        [Fact]
        public async Task AddOrder_InvalidData_ReturnsErrorMessage()
        {
            var controller = new ManagerController(_context);

            var request = new ManagerController.AddOrderRequest
            {
                ClientId = 1,
                WorkerId = 1,
                ServiceId = 0, 
                SelectedComponents = new List<ManagerController.SelectedComponent>
        {
            new ManagerController.SelectedComponent { ComponentId = 1, Quantity = 2 }
        }
            };

            var result = await controller.AddOrder(request);

            var jsonResult = Assert.IsType<JsonResult>(result);
            dynamic response = jsonResult.Value; 

            Assert.False(response.success);
            Assert.Equal("Неверные данные запроса", response.message);
        }
    }
}