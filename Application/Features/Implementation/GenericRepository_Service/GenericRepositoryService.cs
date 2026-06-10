using Application.Features.Definition.Context;
using Application.Features.Definition.GenericRepository;
using Application.Features.Implementation.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Features.Implementation.GenericRepository_Service
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(IApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<T> GetByIdAsync(params object[] keyValues)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.FindAsync(keyValues);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<IEnumerable<T>> TakeAsync(int count)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.Take(count).ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<IEnumerable<T>> TakeAsync(int skip, int take)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.Skip(skip).Take(take).ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await DbLock.Semaphore.WaitAsync();
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            await DbLock.Semaphore.WaitAsync();
            try
            {
                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await DbLock.Semaphore.WaitAsync();
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<T> RemoveAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await DbLock.Semaphore.WaitAsync();
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task RemoveByIdAsync(params object[] keyValues)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var entity = await _dbSet.FindAsync(keyValues);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _context.SaveChangesAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }
    }
}