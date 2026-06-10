using Application.Features.Definition.Context;
using Application.Features.Implementation.Common;
using Application.Features.Implementation.GenericRepository_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Implementation.Customer_Service
{
    public class CustomerService : GenericRepository<Customer>
    {
        public CustomerService(IApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> GetTopCustomersAsync(int count)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .OrderByDescending(p => p.CustomerId)
                    .Take(count)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<IEnumerable<Customer>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllAsync();

            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.Where(c => c.Name.Contains(name)).ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<Customer> GetByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet.FirstOrDefaultAsync(c => c.Phone == phone);
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                var query = _dbSet.Where(c => c.Name == name);
                if (excludeId.HasValue)
                    query = query.Where(c => c.CustomerId != excludeId.Value);
                return await query.AnyAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }

        public async Task<(bool Success, string Message, Customer Customer)> CreateCustomerAsync(Customer customer)
        {
            if (customer == null) return (false, "مشتری نامعتبر است", null);
            if (string.IsNullOrWhiteSpace(customer.Name)) return (false, "نام مشتری نمی‌تواند خالی باشد", null);
            if (await IsNameExistsAsync(customer.Name)) return (false, "مشتری با این نام قبلاً ثبت شده است", null);

            customer.Creationdate = DateTime.Now;
            var created = await AddAsync(customer);
            return (true, "مشتری با موفقیت اضافه شد", created);
        }

        public async Task<(bool Success, string Message, Customer Customer)> UpdateCustomerAsync(Customer customer)
        {
            if (customer == null) return (false, "مشتری نامعتبر است", null);
            if (string.IsNullOrWhiteSpace(customer.Name)) return (false, "نام مشتری نمی‌تواند خالی باشد", null);
            if (await IsNameExistsAsync(customer.Name, customer.CustomerId))
                return (false, "مشتری با این نام قبلاً ثبت شده است", null);

            var existing = await GetByIdAsync(customer.CustomerId);
            if (existing == null) return (false, "مشتری یافت نشد", null);

            customer.Creationdate = existing.Creationdate;
            var updated = await UpdateAsync(customer);
            return (true, "مشتری با موفقیت بروزرسانی شد", updated);
        }

        public async Task<(bool Success, string Message)> DeleteCustomerAsync(int id)
        {
            var customer = await GetByIdAsync(id);
            if (customer == null) return (false, "مشتری یافت نشد");
            await RemoveAsync(customer);
            return (true, "مشتری با موفقیت حذف شد");
        }

        public async Task<IEnumerable<Customer>> GetPagedAsync(int pageNumber, int pageSize)
        {
            await DbLock.Semaphore.WaitAsync();
            try
            {
                return await _dbSet
                    .OrderBy(c => c.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            finally
            {
                DbLock.Semaphore.Release();
            }
        }
    }
}