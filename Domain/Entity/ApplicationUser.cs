using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    // ============================== User ==============================
    public class ApplicationUser: IdentityUser
    {
        
        [MaxLength(50)]
        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

