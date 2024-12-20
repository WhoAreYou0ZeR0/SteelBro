using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Controllers;
using SteelBro.Models;
using System.Security.Claims;
using Xunit;

namespace SteelBro.Tests.Controllers
{
    public class WorkerControllerTests
    {
        private readonly ApplicationDbContext _context;

        public WorkerControllerTests()
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
                Username = "workeruser",
                PasswordHash = "hashedpassword",
                Salt = "salt",
                RoleId = 1,
                Worker = new Worker
                {
                    WorkerId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    MiddleName = "Michael",
                    PhoneNumber = 1234567890,
                    Email = "worker@example.com",
                    PostId = 1,
                    Post = new Post { PostId = 1, PostName = "Technician" }
                }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ValidWorker_ReturnsViewWithWorkerProfile()
        {
            var controller = new WorkerController(_context);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "workeruser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<WorkerProfileViewModel>(viewResult.Model);
            Assert.Equal("Doe John Michael", model.FullName);
            Assert.Equal("workeruser", model.Username);
            Assert.Equal("1234567890", model.PhoneNumber);
            Assert.Equal("worker@example.com", model.Email);
            Assert.Equal("Technician", model.PostName);
        }
    }
}