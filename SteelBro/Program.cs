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

// ���� � ����� ������������
string iniFilePath = "config.ini";

// �������� ������ ����������� �� .ini �����
string connectionString = IniConfigReader.GetConnectionString(iniFilePath);

// ��������� DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ��������� ������� ��������������
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Index"; // ���� � �������� �����
        options.AccessDeniedPath = "/Auth/AccessDenied"; // ���� � �������� ������ � �������
    });

// ��������� ������� �����������
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
    options.AddPolicy("TechnicianOnly", policy => policy.RequireRole("Technician"));
    options.AddPolicy("FinancierOnly", policy => policy.RequireRole("Financier"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client"));
});

// ��������� ��������� ������
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // ����� ����� ������
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ��������� Content-Security-Policy
app.UseMiddleware<CspMiddleware>();



// ��������� HTTP-���������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // ��������� ��������� Strict-Transport-Security
}


app.UseHttpsRedirection(); // ��������������� HTTP �� HTTPS
app.UseStaticFiles(); // ��������� ����������� ������ (CSS, JS, �����������)

app.UseRouting(); // ��������� �������������

app.UseAuthentication(); // ��������� ��������������
app.UseAuthorization(); // ��������� �����������

app.UseSession(); // ��������� ��������� ������


// ��������� ���������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ��������� ������� ��� TechnicianController
app.MapControllerRoute(
    name: "technician",
    pattern: "Technician/{action=Index}/{id?}",
    defaults: new { controller = "Technician" });


// ��������� ������� ��� ManagerController
app.MapControllerRoute(
    name: "manager",
    pattern: "Manager/{action=Index}/{id?}",
    defaults: new { controller = "Manager" });

app.Run();