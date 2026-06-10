using Application.Features.Definition.Context;
using Application.Features.Implementation.Common;
using Application.Features.Implementation.GenericRepository_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Implementation.Supplier_Service
{
    public class SupplierService : GenericRepository<Supplier>
    {
        public SupplierService(IApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Supplier>> GetTopSuppliersAsync(int count)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .OrderByDescending(p => p.SupplierId)
                    .Take(count)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<Supplier> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.FirstOrDefaultAsync(s => s.Name == name);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<Supplier> GetByEconomicCodeAsync(string economicCode)
        {
            if (string.IsNullOrWhiteSpace(economicCode)) return null;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.FirstOrDefaultAsync(s => s.EconomicCode == economicCode);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Supplier>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync(); // GetAllAsync خودش قفل دارد

            keyword = keyword.Trim();
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .Where(s => s.Name.Contains(keyword) ||
                                s.Phone.Contains(keyword) ||
                                s.Address.Contains(keyword))
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsPhoneDuplicateAsync(string phone, int? excludeSupplierId = null)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet.Where(s => s.Phone == phone);
                if (excludeSupplierId.HasValue)
                    query = query.Where(s => s.SupplierId != excludeSupplierId.Value);
                return await query.AnyAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Supplier>> GetWithEconomicCodeAsync()
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.Where(s => !string.IsNullOrWhiteSpace(s.EconomicCode)).ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public override async Task<Supplier> UpdateAsync(Supplier entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await DbLock.Semaphore.WaitAsync();
            try
            {
                if (await IsPhoneDuplicateAsync(entity.Phone, entity.SupplierId))
                    throw new InvalidOperationException("شماره تلفن تکراری است.");

                return await base.UpdateAsync(entity); 
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }
    }
}