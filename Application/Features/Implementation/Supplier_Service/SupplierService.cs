using Application.Features.Definition.Context;
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

        /// <summary>
        /// دریافت تأمین‌کننده بر اساس نام (جستجوی دقیق)
        /// </summary>
        public async Task<Supplier> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return await _dbSet.FirstOrDefaultAsync(s => s.Name == name);
        }

        /// <summary>
        /// دریافت تأمین‌کننده بر اساس شماره اقتصادی
        /// </summary>
        public async Task<Supplier> GetByEconomicCodeAsync(string economicCode)
        {
            if (string.IsNullOrWhiteSpace(economicCode))
                return null;
            return await _dbSet.FirstOrDefaultAsync(s => s.EconomicCode == economicCode);
        }

        /// <summary>
        /// جستجوی تأمین‌کنندگان با کلمه کلیدی (نام، تلفن، آدرس)
        /// </summary>
        public async Task<IEnumerable<Supplier>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync();

            keyword = keyword.Trim();
            return await _dbSet
                .Where(s => s.Name.Contains(keyword) ||
                            s.Phone.Contains(keyword) ||
                            s.Address.Contains(keyword))
                .ToListAsync();
        }

        /// <summary>
        /// بررسی تکراری بودن شماره تلفن (برای جلوگیری از ثبت تکراری)
        /// </summary>
        public async Task<bool> IsPhoneDuplicateAsync(string phone, int? excludeSupplierId = null)
        {
            var query = _dbSet.Where(s => s.Phone == phone);
            if (excludeSupplierId.HasValue)
                query = query.Where(s => s.SupplierId != excludeSupplierId.Value);
            return await query.AnyAsync();
        }

        /// <summary>
        /// دریافت تأمین‌کنندگانی که کد اقتصادی آنها مشخص است (غیر خالی)
        /// </summary>
        public async Task<IEnumerable<Supplier>> GetWithEconomicCodeAsync()
        {
            return await _dbSet.Where(s => !string.IsNullOrWhiteSpace(s.EconomicCode)).ToListAsync();
        }

        // override UpdateAsync برای اعتبارسنجی اضافی
        public override async Task<Supplier> UpdateAsync(Supplier entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // جلوگیری از ثبت تلفن تکراری (به جز خود تأمین‌کننده)
            if (await IsPhoneDuplicateAsync(entity.Phone, entity.SupplierId))
                throw new InvalidOperationException("شماره تلفن تکراری است.");

            return await base.UpdateAsync(entity);
        }
    }
}