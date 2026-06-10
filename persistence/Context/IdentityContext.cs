using Application.Features.Definition.Context;
using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class IdentityContext : IdentityDbContext<ApplicationUser,Role,string>, IApplicationDbContext
{
    // سازنده برای استفاده در برنامه (با تزریق وابستگی)
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    // سازنده بدون پارامتر برای ابزارهای مهاجرت
    public IdentityContext() : base()
    {
    }

    // متد پیکربندی برای زمان‌هایی که گزینه‌ها از قبل تنظیم نشده‌اند
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    public new DbSet<T> Set<T>() where T : class => base.Set<T>();
    public new async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
    public new int SaveChanges() => base.SaveChanges();
}