using Microsoft.EntityFrameworkCore;
using SteelBro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SteelBro.Middlewear;
using System.Globalization;


var cultureInfo = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Путь к файлу конфигурации
string iniFilePath = "config.ini";

// Получаем строку подключения из .ini файла
string connectionString = IniConfigReader.GetConnectionString(iniFilePath);

// Настройка DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Добавляем сервисы аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Index"; // Путь к странице входа
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Путь к странице отказа в доступе
    });

// Настройка политик авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
    options.AddPolicy("TechnicianOnly", policy => policy.RequireRole("Technician"));
    options.AddPolicy("FinancierOnly", policy => policy.RequireRole("Financier"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client"));
});

// Добавляем поддержку сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Настройка Content-Security-Policy
app.UseMiddleware<CspMiddleware>();



// Настройка HTTP-конвейера
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Добавляем поддержку Strict-Transport-Security
}


app.UseHttpsRedirection(); // Перенаправление HTTP на HTTPS
app.UseStaticFiles(); // Поддержка статических файлов (CSS, JS, изображения)

app.UseRouting(); // Поддержка маршрутизации

app.UseAuthentication(); // Добавляем аутентификацию
app.UseAuthorization(); // Добавляем авторизацию

app.UseSession(); // Добавляем поддержку сессий


// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Добавляем маршрут для TechnicianController
app.MapControllerRoute(
    name: "technician",
    pattern: "Technician/{action=Index}/{id?}",
    defaults: new { controller = "Technician" });


// Добавляем маршрут для ManagerController
app.MapControllerRoute(
    name: "manager",
    pattern: "Manager/{action=Index}/{id?}",
    defaults: new { controller = "Manager" });

app.Run();