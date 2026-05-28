using Application.Features.Definition.GenericRepository;
using Application.Features.Implementation.Category_Service;
using Application.Features.Implementation.Customer_Service;
using Application.Features.Implementation.GenericRepository_Service;
using Application.Features.Implementation.GoodsIssue_Service;
using Application.Features.Implementation.GoodsReceipt_Service;
using Application.Features.Implementation.Product_Service;
using Application.Features.Implementation.Supplier_Service;
using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistans.Context;
using persistence.Context;
using StoreRoom.Forms;
using System;
using System.Windows.Forms;

namespace StoreRoom
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            // 1. ثبت DbContextهای برنامه
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;"));

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer("Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;"));

            // 2. ثبت Identity با AddIdentityCore (نیاز به پکیج Microsoft.AspNetCore.Identity ندارد)
            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // 3. همچنین باید SignInManager را به صورت دستی اضافه کنیم
            services.AddScoped<SignInManager<IdentityUser>>();

            // 4. تنظیمات اختیاری Password
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            // 5. ثبت سرویس‌های سفارشی
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<WarehouseService>();
            services.AddScoped<SupplierService>();
            services.AddScoped<GoodsReceiptService>();
            services.AddScoped<GoodsIssueService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ProductService>();
            services.AddScoped<CategoryService>();

            // 6. ساخت ServiceProvider
            ServiceProvider = services.BuildServiceProvider();

            // 7. اجرای فرم اصلی (حتماً Form1 باید در DI ثبت شده باشد)
            var mainForm = ServiceProvider.GetRequiredService<Form1>();
            System.Windows.Forms.Application.Run(mainForm);
            // ... بعد از ثبت سرویس‌ها، قبل از BuildServiceProvider
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
        }
    }
}