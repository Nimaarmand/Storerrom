using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace StoreRoom.Data
{
    public class IdentityDatabaseSeeder
    {
        private readonly string _connectionString;

        public IdentityDatabaseSeeder(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SeedAllRolesAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            using var context = new IdentityContext(optionsBuilder.Options);
            var roleManager = CreateRoleManager(context);

            var roles = new (string Name, string Description)[]
            {
                ("Admin", "دسترسی کامل به تمام بخش‌های سیستم، مدیریت کاربران، تنظیمات و گزارشات"),
                ("Manager", "مدیریت انبارها، تأمین‌کنندگان، محصولات و مشاهده گزارشات (بدون دسترسی به مدیریت کاربران)"),
                ("WarehouseKeeper", "ثبت رسید و حواله، مدیریت موجودی انبار، مشاهده لیست کالاها"),
                ("Auditor", "مشاهده گزارشات و تراکنش‌های انبار (فقط خواندنی)"),
                ("User", "دسترسی پایه: مشاهده موجودی و جستجو (ثبت تراکنش‌ها نیاز به تأیید مدیر)")
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    var newRole = new Role
                    {
                        Name = role.Name,
                        Discription = role.Description
                    };
                    await roleManager.CreateAsync(newRole);
                }
            }
        }

        public async Task SeedAdminUserAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            using var context = new IdentityContext(optionsBuilder.Options);

            var userManager = CreateUserManager(context);
            var roleManager = CreateRoleManager(context);

            // اطمینان از وجود نقش Admin
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role { Name = "Admin", Discription = "مدیر کل سیستم" });
            }

            string adminUserName = "09010588129";
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    FullName = "نیما آرمند",
                    IsActive = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private UserManager<ApplicationUser> CreateUserManager(IdentityContext context)
        {
            // ✅ استفاده از UserStore با انواع جنریک کامل (ApplicationUser, Role, IdentityContext, string)
            var store = new UserStore<ApplicationUser, Role, IdentityContext, string>(context);
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() };

            return new UserManager<ApplicationUser>(
                store, null, passwordHasher, userValidators, passwordValidators, null, null, null, null);
        }

        private RoleManager<Role> CreateRoleManager(IdentityContext context)
        {
            var store = new RoleStore<Role, IdentityContext, string>(context);
            return new RoleManager<Role>(store, null, null, null, null);
        }
    }
}