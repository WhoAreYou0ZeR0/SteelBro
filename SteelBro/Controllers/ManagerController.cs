using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteelBro.Models;

namespace SteelBro.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("Manager")]
    public class ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Отображение страницы с клиентами
        [HttpGet("Clients")]
        public IActionResult Clients()
        {
            return View();
        }

        // Отображение страницы с работниками
        [HttpGet("Workers")]
        public IActionResult Workers()
        {
            return View();
        }

        // Отображение страницы с заказами
        [HttpGet("Index")]
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
        [HttpGet("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _context.Status.ToListAsync();
            return Json(statuses);
        }

        // Получение списка заказов с фильтрацией
        [HttpGet("GetOrders")]
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

        // Просмотр заказа
        [HttpGet("ViewOrder/{id}")]
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
        [HttpPost("UpdateOrderStatus")]
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

        // Получение списка клиентов с фильтрацией
        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients(string clientName = null)
        {
            var clients = _context.Clients
                .Include(c => c.User) // Подключаем данные пользователя клиента
                .AsQueryable();

            if (!string.IsNullOrEmpty(clientName))
            {
                clients = clients.Where(c => c.FirstName.Contains(clientName) || c.LastName.Contains(clientName) || c.MiddleName.Contains(clientName));
            }

            var result = await clients.Select(c => new
            {
                c.ClientId,
                FullName = $"{c.LastName} {c.FirstName} {c.MiddleName}",
                c.PhoneNumber,
                c.Email
            }).ToListAsync();

            return Json(result);
        }

        // Просмотр клиента
        [HttpGet("ViewClient/{id}")]
        public async Task<IActionResult> ViewClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.User) // Подключаем данные пользователя клиента
                .Include(c => c.Orders) // Подключаем данные заказов клиента
                    .ThenInclude(o => o.Service) // Подключаем данные услуги
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Worker) // Подключаем данные работника
                .Include(c => c.Orders)
                    .ThenInclude(o => o.ComponentOrders) // Подключаем данные компонентов заказа
                        .ThenInclude(co => co.Component) // Подключаем данные компонентов
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // Добавление заказа
        [HttpGet("AddOrder")]
        public async Task<IActionResult> AddOrder(int clientId)
        {
            // Загружаем список услуг
            var services = await _context.Services.ToListAsync();
            ViewBag.Services = services;

            // Загружаем список работников
            var workers = await _context.Workers.ToListAsync();
            ViewBag.Workers = workers;

            // Загружаем список комплектующих
            var components = await _context.Components.ToListAsync();
            ViewBag.Components = components;

            // Передаем clientId в ViewBag
            ViewBag.ClientId = clientId;

            return View();
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            if (request == null || request.ServiceId <= 0 || request.WorkerId <= 0 || request.ClientId <= 0)
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            // Получаем статус "Новый" (или другой статус по умолчанию)
            var defaultStatus = await _context.Status.FirstOrDefaultAsync(s => s.StatusName == "Новый заказ");
            if (defaultStatus == null)
            {
                return Json(new { success = false, message = "Статус по умолчанию не найден" });
            }

            // Создаем новый заказ
            var newOrder = new Order
            {
                ServiceId = request.ServiceId,
                WorkerId = request.WorkerId,
                StatusId = defaultStatus.StatusId,
                OrderDate = DateTime.Now,
                ClientId = request.ClientId
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            // Добавляем комплектующие к заказу
            if (request.SelectedComponents != null)
            {
                foreach (var component in request.SelectedComponents)
                {
                    var componentOrder = new ComponentOrder
                    {
                        OrderId = newOrder.OrderId,
                        ComponentId = component.ComponentId,
                        Quantity = component.Quantity
                    };

                    _context.ComponentOrder.Add(componentOrder);
                }

                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Заказ успешно добавлен", orderId = newOrder.OrderId });
        }

        // Вспомогательные классы для запросов
        public class UpdateStatusRequest
        {
            public int OrderId { get; set; }
            public string NewStatus { get; set; }
        }

        public class AddOrderRequest
        {
            public int ServiceId { get; set; }
            public int WorkerId { get; set; }
            public int ClientId { get; set; }
            public List<SelectedComponent> SelectedComponents { get; set; }
        }

        public class SelectedComponent
        {
            public int ComponentId { get; set; }
            public int Quantity { get; set; }
        }
        // Получение списка работников с фильтрацией
        [HttpGet("GetWorkers")]
        public async Task<IActionResult> GetWorkers(string workerName = null)
        {
            var workers = _context.Workers
                .Include(w => w.User) // Подключаем данные пользователя работника
                .AsQueryable();

            if (!string.IsNullOrEmpty(workerName))
            {
                workers = workers.Where(w => w.FirstName.Contains(workerName) || w.LastName.Contains(workerName) || w.MiddleName.Contains(workerName));
            }

            var result = await workers.Select(w => new
            {
                w.WorkerId,
                FullName = $"{w.LastName} {w.FirstName} {w.MiddleName}",
                w.PhoneNumber,
                w.Email,
                w.Post.PostName,
                RoleName = w.User.Role.RoleName
            }).ToListAsync();

            return Json(result);
        }

        // Просмотр работника
        [HttpGet("ViewWorker/{id}")]
        public async Task<IActionResult> ViewWorker(int id)
        {
            var worker = await _context.Workers
                .Include(w => w.User) // Загружаем пользователя
                    .ThenInclude(u => u.Role) // Загружаем роль пользователя
                .Include(w => w.Post) // Загружаем должность
                .FirstOrDefaultAsync(w => w.WorkerId == id);

            if (worker == null)
            {
                return NotFound(); // Если работник не найден, возвращаем 404
            }

            // Загружаем роли и должности для выпадающих списков
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = roles;

            var posts = await _context.Post.ToListAsync();
            ViewBag.Posts = posts;

            return View(worker);
        }

        // Обновление роли работника
        [HttpPost("UpdateWorkerRole")]
        public async Task<IActionResult> UpdateWorkerRole([FromBody] UpdateWorkerRoleRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.NewRole))
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var worker = await _context.Workers
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.WorkerId == request.WorkerId);

            if (worker == null)
            {
                return Json(new { success = false, message = "Работник не найден" });
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.NewRole);
            if (role == null)
            {
                return Json(new { success = false, message = "Указанная роль не существует" });
            }

            worker.User.Role = role;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Роль работника обновлена", updatedRole = role.RoleName });
        }

        // Обновление должности работника
        [HttpPost("UpdateWorkerPost")]
        public async Task<IActionResult> UpdateWorkerPost([FromBody] UpdateWorkerPostRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.NewPost))
            {
                return Json(new { success = false, message = "Неверные данные запроса" });
            }

            var worker = await _context.Workers
                .Include(w => w.Post)
                .FirstOrDefaultAsync(w => w.WorkerId == request.WorkerId);

            if (worker == null)
            {
                return Json(new { success = false, message = "Работник не найден" });
            }

            var post = await _context.Post.FirstOrDefaultAsync(p => p.PostName == request.NewPost);
            if (post == null)
            {
                return Json(new { success = false, message = "Указанная должность не существует" });
            }

            worker.Post = post;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Должность работника обновлена", updatedPost = post.PostName });
        }

        // Вспомогательные классы для запросов
        public class UpdateWorkerRoleRequest
        {
            public int WorkerId { get; set; }
            public string NewRole { get; set; }
        }

        public class UpdateWorkerPostRequest
        {
            public int WorkerId { get; set; }
            public string NewPost { get; set; }
        }

    }
}