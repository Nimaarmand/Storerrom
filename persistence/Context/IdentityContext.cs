using Application.Features.Definition.Context;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace persistence.Context
{


    public class IdentityContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=StoreroomDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

       
        public new DbSet<T> Set<T>() where T : class => base.Set<T>();
        public new async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
        public new int SaveChanges() => base.SaveChanges();
    }
}
