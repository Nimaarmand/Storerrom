using Application.Features.Definition.Context;
using Application.Features.Definition.GenericRepository;
using Application.Features.Implementation.Category_Service;
using Application.Features.Implementation.Customer_Service;
using Application.Features.Implementation.GenericRepository_Service;
using Application.Features.Implementation.GoodsIssue_Service;
using Application.Features.Implementation.GoodsReceipt_Service;
using Application.Features.Implementation.Product_Service;
using Application.Features.Implementation.Supplier_Service;
using Application.Features.Implementation.Usermanagement_Service;
using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistans.Context;
using StoreRoom.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace StoreRoom
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static string CurrentUserId { get; set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            string connectionString = "Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;";

            // 1. DbContextها
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // 2. Identity (UserManager, RoleManager به صورت Scoped)
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // 3. Data Protection
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StoreroomKeys")))
                .SetApplicationName("StoreroomApp");

            // 4. تنظیمات رمز عبور
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = false;
            });

            // 5. سرویس‌های سفارشی
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<WarehouseService>();
            services.AddScoped<SupplierService>();
            services.AddScoped<GoodsReceiptService>();
            services.AddScoped<GoodsIssueService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ProductService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<UsermanagementService>();

            // 6. ثبت فرم‌ها به صورت Transient
            services.AddTransient<Form1>();
            services.AddTransient<Form2>();
            // ... سایر فرم‌ها
            services.AddTransient<Form17>();
            services.AddTransient<Form18>();

            ServiceProvider = services.BuildServiceProvider();

            var mainForm = ServiceProvider.GetRequiredService<Form1>();
           System.Windows.Forms.Application.Run(mainForm);
        }
    }
}