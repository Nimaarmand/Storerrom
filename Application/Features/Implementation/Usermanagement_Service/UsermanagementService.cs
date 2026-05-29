using Application.Features.Definition.Usermanagement_Service;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Implementation.Usermanagement_Service
{
    public class UsermanagementService: IUserManagement
    {
        
        public UsermanagementService()
        {
            
        }

        public Task<IdentityResult> ActivateUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeactivateUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindUserByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<SignInResult> LoginAsync(string userName, string password, bool rememberMe = false, bool lockoutOnFailure = false)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string role = null)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistsAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
