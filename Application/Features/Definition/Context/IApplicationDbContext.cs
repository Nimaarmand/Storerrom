using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Definition.Context
{
    public interface IApplicationDbContext
    {
        DbSet<T> Set<T>() where T : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
