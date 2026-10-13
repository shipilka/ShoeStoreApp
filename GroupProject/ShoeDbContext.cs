using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GroupProject
{
    // Главный контекст базы данных MS SQL
    public class ShoeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Строка подключения к базе данных MS SQL (ispp3401)
                optionsBuilder.UseSqlServer("Server=localhost;Database=ispp3401;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderDetail>().HasKey(od => od.OrderDetailId);
            modelBuilder.Entity<OrderStatus>().HasKey(os => os.OrderStatusId);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);

            // Связывающая таблица UserRoles
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));
        }
    }

    // 1. Пользователь
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}".Trim();
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }

    // 2. Роль
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public ICollection<User> Users { get; set; } = new List<User>();
    }

    // 3. Товар (Обувь)
    public class Product
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Manufacturer { get; set; }
        public string Category { get; set; } = null!;
        public string? Subcategory { get; set; }
        public int? ActiveDiscount { get; set; }
        public int? StockQuantity { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public int? SizeRange { get; set; }
        public int? AvailableSizes { get; set; }

        // Синонимы для поддержки имён из XAML
        public string Name => ProductName;
        public int Stock => StockQuantity ?? 0;

        // Вычисляемые свойства
        public decimal DiscountedPrice => ActiveDiscount.HasValue && ActiveDiscount > 0
            ? Price * (1 - (decimal)ActiveDiscount.Value / 100)
            : Price;

        public string StockStatus => (StockQuantity ?? 0) > 5 ? "много" : "мало";

        public bool IsLowStock => (StockQuantity ?? 0) <= 3;
    }

    // 4. Заказ
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int PickupCode { get; set; }
        public int OrderStatusId { get; set; }
        public decimal? TotalAmount { get; set; }

        public User? User { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    // 5. Детали заказа
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }

    // 6. Статус заказа
    public class OrderStatus
    {
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; } = null!;
    }

    public struct Db { }
}