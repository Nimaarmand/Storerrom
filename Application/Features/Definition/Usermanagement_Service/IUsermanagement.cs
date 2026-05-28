using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Definition.Usermanagement_Service
{
    using Domain.Entity;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserManagement
    {
        // ثبت‌نام کاربر جدید
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string role = null);

        // ورود به سیستم (دریافت نتیجه موفقیت‌آمیز)
        Task<SignInResult> LoginAsync(string userName, string password, bool rememberMe = false, bool lockoutOnFailure = false);

        // خروج از سیستم
        Task LogoutAsync();

        // تغییر رمز عبور با تأیید رمز فعلی
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        // بازنشانی رمز عبور (تولید توکن و تنظیم رمز جدید)
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);

        // ایجاد توکن بازنشانی رمز عبور (برای ارسال ایمیل)
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

        // دریافت کاربر بر اساس نام کاربری یا ایمیل
        Task<ApplicationUser> FindUserByNameAsync(string userName);
        Task<ApplicationUser> FindUserByEmailAsync(string email);

        // دریافت کاربر بر اساس شناسه
        Task<ApplicationUser> FindUserByIdAsync(string userId);

        // لیست تمام کاربران (اختیاری با فیلتر صفحه‌بندی)
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        // حذف کاربر (فیزیکی یا غیرفعال کردن)
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);

        // غیرفعال/فعال کردن کاربر (در صورت وجود فیلد IsActive)
        Task<IdentityResult> DeactivateUserAsync(string userId);
        Task<IdentityResult> ActivateUserAsync(string userId);

        // مدیریت نقش‌ها (اختصاص نقش یا حذف نقش)
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);

        // جستجوی کاربران بر اساس کلمه کلیدی (در فیلدهای نام، نام کاربری، ایمیل، ...)
        Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string keyword);

        // بررسی وجود کاربر (مثلاً برای اعتبارسنجی)
        Task<bool> UserExistsAsync(string userName);
        Task<bool> EmailExistsAsync(string email);
    }
}
