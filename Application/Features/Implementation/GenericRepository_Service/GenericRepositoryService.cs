using Application.Features.Definition.Context;
using Application.Features.Definition.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        /// <summary>
        /// دریافت تعداد مشخصی رکورد از ابتدا (مثل TOP N)
        /// </summary>
        public virtual async Task<IEnumerable<T>> TakeAsync(int count)
        {
            return await _dbSet.Take(count).ToListAsync();
        }

        /// <summary>
        /// دریافت تعداد مشخصی رکورد با رد شدن از تعدادی رکورد (برای صفحه‌بندی)
        /// </summary>
        public virtual async Task<IEnumerable<T>> TakeAsync(int skip, int take)
        {
            return await _dbSet.Skip(skip).Take(take).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            await _dbSet.AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> RemoveAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task RemoveByIdAsync(params object[] keyValues)
        {
            var entity = await GetByIdAsync(keyValues);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
