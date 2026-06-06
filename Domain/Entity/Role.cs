using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class Role:IdentityRole
    {
        public string Discription { get; set; }
    }
}
