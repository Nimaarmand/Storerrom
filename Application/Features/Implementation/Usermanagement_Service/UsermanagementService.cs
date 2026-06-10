using Application.Constants.User_error_handling;
using Application.Features.Definition.Usermanagement_Service;
using Application.Features.Implementation.Common;
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
        private readonly RoleManager<Role> _roleManager;

        public UsermanagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> GetUserNameByIdAsync(string userId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(userId)) return "نامشخص";
                var user = await _userManager.FindByIdAsync(userId);
                return user?.UserName ?? "نامشخص";
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // فعال کردن کاربر
        public async Task<IdentityResult> ActivateUserAsync(string userId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                user.IsActive = true;
                return await _userManager.UpdateAsync(user);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // غیرفعال کردن کاربر
        public async Task<IdentityResult> DeactivateUserAsync(string userId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                user.IsActive = false;
                return await _userManager.UpdateAsync(user);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // حذف کاربر
        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                return await _userManager.DeleteAsync(user);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // تغییر رمز عبور
        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // ثبت‌نام کاربر جدید
        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string role = null)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.InvalidUser });

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded && !string.IsNullOrWhiteSpace(role))
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        await _roleManager.CreateAsync(new Role { Name = role });
                    await _userManager.AddToRoleAsync(user, role);
                }
                return result;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // ورود (بررسی رمز عبور)
        public async Task<bool> LoginAsync(string userName, string password, bool rememberMe = false, bool lockoutOnFailure = false)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null) return false;
                return await _userManager.CheckPasswordAsync(user, password);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // خروج (در ویندوز فرم کاری انجام نمی‌دهد)
        public async Task LogoutAsync() => await Task.CompletedTask;

        // جستجو بر اساس شناسه
        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _userManager.FindByIdAsync(userId);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string userName)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _userManager.FindByNameAsync(userName);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(email)) return null;
                return await _userManager.FindByEmailAsync(email);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // دریافت همه کاربران
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _userManager.Users.ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // جستجوی کاربران
        public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string keyword)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllUsersAsync();

                keyword = keyword.Trim().ToLower();
                return await _userManager.Users
                    .Where(u => u.UserName.ToLower().Contains(keyword) ||
                                (u.FullName != null && u.FullName.ToLower().Contains(keyword)))
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _userManager.FindByNameAsync(userName) != null;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(email)) return false;
                return await _userManager.FindByEmailAsync(email) != null;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // توکن بازنشانی رمز
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                return await _userManager.ResetPasswordAsync(user, token, newPassword);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        #region User Roles Management

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                if (string.IsNullOrWhiteSpace(role))
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.InvalidRole });

                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new Role { Name = role });

                return await _userManager.AddToRoleAsync(user, role);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null)
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.UserNotFound });
                if (string.IsNullOrWhiteSpace(role))
                    return IdentityResult.Failed(new IdentityError { Description = MessageUsermanagemen.InvalidRole });
                return await _userManager.RemoveFromRoleAsync(user, role);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null) return new List<string>();
                return await _userManager.GetRolesAsync(user);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (user == null || string.IsNullOrWhiteSpace(role)) return false;
                return await _userManager.IsInRoleAsync(user, role);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        #endregion

        #region Role Management

        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (role == null)
                    return IdentityResult.Failed(new IdentityError { Description = "نقش نمی‌تواند خالی باشد." });
                if (string.IsNullOrWhiteSpace(role.Name))
                    return IdentityResult.Failed(new IdentityError { Description = "نام نقش نمی‌تواند خالی باشد." });

                if (await _roleManager.RoleExistsAsync(role.Name))
                    return IdentityResult.Failed(new IdentityError { Description = "نقش قبلاً وجود دارد." });

                return await _roleManager.CreateAsync(role);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _roleManager.RoleExistsAsync(roleName);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        // خروجی IEnumerable<Role>
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _roleManager.Roles.ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            await DbLock.Semaphore.WaitAsync();
            try
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
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        #endregion
    }
}