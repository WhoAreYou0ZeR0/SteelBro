using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace SteelBro.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet для таблиц
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ComponentOrder> ComponentOrder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка составного первичного ключа для ComponentOrder
            modelBuilder.Entity<ComponentOrder>()
                .HasKey(co => new { co.OrderId, co.ComponentId });

            // Настройка связи между ComponentOrder и Order
            modelBuilder.Entity<ComponentOrder>()
                .HasOne(co => co.Order)
                .WithMany(o => o.ComponentOrders)
                .HasForeignKey(co => co.OrderId);

            // Настройка связи между ComponentOrder и Component
            modelBuilder.Entity<ComponentOrder>()
                .HasOne(co => co.Component)
                .WithMany(c => c.ComponentOrders)
                .HasForeignKey(co => co.ComponentId);

            // Настройка связи между Order и Client
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId);

            // Настройка связи между Order и Worker
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Worker)
                .WithMany(w => w.Orders)
                .HasForeignKey(o => o.WorkerId);

            // Настройка связи между Order и Status
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StatusId);

            // Настройка связи между Order и Service
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId);

            // Настройка связи между Client и User
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.UserId);

            // Настройка связи между Worker и User
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.User)
                .WithOne(u => u.Worker)
                .HasForeignKey<Worker>(w => w.UserId);

            // Настройка связи между Worker и Post
            modelBuilder.Entity<Worker>()
                .HasOne(w => w.Post)
                .WithMany(p => p.Workers)
                .HasForeignKey(w => w.PostId);

            // Настройка связи между User и Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Status>().HasData(
        new Status { StatusId = 1, StatusName = "Новый заказ" });
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
    }
}