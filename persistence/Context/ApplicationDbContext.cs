using Microsoft.EntityFrameworkCore;
using Domain.Entity;
using Application.Feature.Definition.Context;


namespace Persistans.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               
                optionsBuilder.UseSqlServer("Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public DbSet<GoodsIssue> GoodsIssues { get; set; }
        public DbSet<Category> Categories { get; set; }

        // پیاده‌سازی اینترفیس
        public new DbSet<T> Set<T>() where T : class => base.Set<T>();
        public new async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
        public new int SaveChanges() => base.SaveChanges();
    }
}