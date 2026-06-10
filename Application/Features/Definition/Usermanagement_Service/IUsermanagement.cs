using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Features.Definition.Usermanagement_Service
{
    public interface IUserManagement
    {
        // مدیریت کاربران
        Task<IdentityResult> ActivateUserAsync(string userId);
        Task<IdentityResult> DeactivateUserAsync(string userId);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string role = null);
        Task<bool> LoginAsync(string userName, string password, bool rememberMe = false, bool lockoutOnFailure = false);
        Task LogoutAsync();
        Task<ApplicationUser> FindUserByIdAsync(string userId);
        Task<ApplicationUser> FindUserByNameAsync(string userName);
        Task<ApplicationUser> FindUserByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string keyword);
        Task<bool> UserExistsAsync(string userName);
        Task<bool> EmailExistsAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);

        // مدیریت نقش‌های کاربر
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);

        // مدیریت نقش‌ها (Roles) - با استفاده از Role سفارشی
        Task<IdentityResult> CreateRoleAsync(Role role);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IEnumerable<Role>> GetAllRolesAsync();  // تغییر کلیدی
        Task<IdentityResult> DeleteRoleAsync(string roleName);
    }
}