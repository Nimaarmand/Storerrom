using Application.Constants.User_error_handling;
using Application.Features.Definition.Usermanagement_Service;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Implementation.Usermanagement_Service
{
    public class UsermanagementService : IUserManagement
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsermanagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // فعال کردن کاربر
        public async Task<IdentityResult> ActivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            user.IsActive = true;
            return await _userManager.UpdateAsync(user);
        }

        // غیرفعال کردن کاربر
        public async Task<IdentityResult> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            user.IsActive = false;
            return await _userManager.UpdateAsync(user);
        }

        // حذف کاربر
        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            return await _userManager.DeleteAsync(user);
        }

        // تغییر رمز عبور
        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        // ثبت‌نام کاربر جدید
        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string role = null)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.InvalidUser });

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded && !string.IsNullOrWhiteSpace(role))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
                await _userManager.AddToRoleAsync(user, role);
            }
            return result;
        }

        // ورود (بررسی رمز عبور)
        public async Task<bool> LoginAsync(string userName, string password, bool rememberMe = false, bool lockoutOnFailure = false)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return false;
            return await _userManager.CheckPasswordAsync(user, password);
        }

        // خروج (در ویندوز فرم کاری انجام نمی‌دهد)
        public async Task LogoutAsync() => await Task.CompletedTask;

        // جستجو بر اساس شناسه
        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _userManager.FindByEmailAsync(email);
        }

        // دریافت همه کاربران
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        // جستجوی کاربران
        public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllUsersAsync();

            keyword = keyword.Trim().ToLower();
            return await _userManager.Users
                .Where(u => u.UserName.ToLower().Contains(keyword) ||
                            (u.FullName != null && u.FullName.ToLower().Contains(keyword)))
                .ToListAsync();
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return await _userManager.FindByEmailAsync(email) != null;
        }

        // توکن بازنشانی رمز
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        #region User Roles Management

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            if (string.IsNullOrWhiteSpace(role))
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.InvalidRole });

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
            if (string.IsNullOrWhiteSpace(role))
                return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.InvalidRole });
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            if (user == null) return new List<string>();
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            if (user == null || string.IsNullOrWhiteSpace(role)) return false;
            return await _userManager.IsInRoleAsync(user, role);
        }

        #endregion

        #region Role Management

        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "نقش نمی‌تواند خالی باشد." });
            if (string.IsNullOrWhiteSpace(role.Name))
                return IdentityResult.Failed(new IdentityError { Description = "نام نقش نمی‌تواند خالی باشد." });

            if (await _roleManager.RoleExistsAsync(role.Name))
                return IdentityResult.Failed(new IdentityError { Description = "نقش قبلاً وجود دارد." });

            return await _roleManager.CreateAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return IdentityResult.Failed(new IdentityError { Description = "نام نقش نامعتبر است." });

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "نقش یافت نشد." });

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (usersInRole.Any())
                return IdentityResult.Failed(new IdentityError { Description = "کاربرانی با این نقش وجود دارند. ابتدا نقش آن‌ها را تغییر دهید." });

            return await _roleManager.DeleteAsync(role);
        }

        #endregion
    }
}