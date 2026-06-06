using Application.Features.Definition.Context;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Persistans.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        // سازنده اصلی برای استفاده از DI (در برنامه)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // سازنده بدون پارامتر برای ابزارهای مهاجرت (طراحی)
        public ApplicationDbContext()
        {
        }

        // متد OnConfiguring – اگر گزینه‌ها قبلاً تنظیم نشده باشند، از این کانکشن استفاده کن
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        // DbSet ها
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public DbSet<GoodsIssue> GoodsIssues { get; set; }
        public DbSet<Category> Categories { get; set; }

        // پیاده‌سازی اینترفیس IApplicationDbContext
        public new DbSet<T> Set<T>() where T : class => base.Set<T>();
        public new async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
        public new int SaveChanges() => base.SaveChanges();
    }
}