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
using StoreRoom.Data;
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
            string connectionString = "Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;";
            ApplicationConfiguration.Initialize();

            // ========== اجرای Seeder (قبل از هر چیزی) ==========
            var seeder = new IdentityDatabaseSeeder(connectionString);
            seeder.SeedAllRolesAsync().GetAwaiter().GetResult();
            seeder.SeedAdminUserAsync().GetAwaiter().GetResult();

            var services = new ServiceCollection();

            // 1. DbContextها
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // 2. Identity
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<Role>()
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

            // 6. فرم‌ها
            services.AddTransient<Form1>();
            services.AddTransient<Form2>();
            services.AddTransient<Form3>();
            services.AddTransient<Form4>();
            services.AddTransient<Form5>();
            services.AddTransient<Form6>();
            services.AddTransient<Form7>();
            services.AddTransient<Form8>();
            services.AddTransient<Form9>();
            services.AddTransient<Form10>();
            services.AddTransient<Form11>();
            services.AddTransient<Form12>();
            services.AddTransient<Form13>();
            services.AddTransient<Form14>();  
            services.AddTransient<Form15>();
            services.AddTransient<Form16>();
            services.AddTransient<Form17>();
            services.AddTransient<Form18>();
            services.AddTransient<Form19>();
            services.AddTransient<Form20>();
            services.AddTransient<Form21>();
            services.AddTransient<Form22>();
            services.AddTransient<Form23>();
            services.AddTransient<Form24>();
            services.AddTransient<Form25>(); 
            services.AddTransient<Form15>();

            ServiceProvider = services.BuildServiceProvider();

            // نمایش فرم اسپلش (Modal) و منتظر ماندن برای بسته شدن آن
            using (var splash = ServiceProvider.GetRequiredService<Form25>())
            {
                splash.ShowDialog();
            }

            // بعد از بسته شدن اسپلش، فرم لاگین را با Application.Run اجرا کن
            var loginForm = ServiceProvider.GetRequiredService<Form14>();
           System.Windows.Forms.  Application.Run(loginForm);
        }
    }
}